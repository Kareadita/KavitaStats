using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KavitaStats.Data;
using KavitaStats.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KavitaStats.Services;
#nullable enable

public interface ITaskScheduler
{
    void ScheduleTasks();
}

public class TaskScheduler : ITaskScheduler
{
    private readonly ILogger<TaskScheduler> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private static readonly RecurringJobOptions RecurringJobOptions = new RecurringJobOptions()
    {
        TimeZone = TimeZoneInfo.Local
    };

    public TaskScheduler(ILogger<TaskScheduler> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void ScheduleTasks()
    {
        RecurringJob.AddOrUpdate("cleanup-records", () => CleanupRecords(),
            Cron.Daily, RecurringJobOptions);        
        
        RecurringJob.AddOrUpdate<HistoricalSnapshotService>("historical-snapshot", job => job.TakeHistoricalSnapshot(),
            Cron.Weekly, RecurringJobOptions);
    }

    public async Task CleanupRecords()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var services = scope.ServiceProvider;
        var v2Context = services.GetRequiredService<DataContext>();
        var v3Context = services.GetRequiredService<DataContextV3>();
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();
        var serverStatRepository = services.GetRequiredService<IServerStatRepository>();

        var oneYearAgo = DateTime.UtcNow.AddYears(-1);

        var upgradedRecordsDeleted = await RemoveV2RecordsThatMigratedToV3(v2Context, v3Context);
        var staleV2RecordsDeleted = await unitOfWork.StatRecordRepository.DeleteWhereAsync(s => s.LastModified < oneYearAgo);
        var staleV3RecordsDeleted = await serverStatRepository.DeleteWhereAsync(s => s.LastModified < oneYearAgo);

        _logger.LogInformation(
            "Completed cleanup: Deleted {UpgradedCount} records that upgraded to v3, {StaleV2Count} stale v2 records, {StaleV3Count} stale v3 records",
            upgradedRecordsDeleted, staleV2RecordsDeleted, staleV3RecordsDeleted);
    }

    private static async Task<int> RemoveV2RecordsThatMigratedToV3(DataContext v2Context, DataContextV3 v3Context)
    {
        const int batchSize = 500;

        var v3InstallIds = await v3Context.ServerStat
            .Select(s => s.InstallId)
            .Distinct()
            .ToListAsync();

        var totalDeleted = 0;
        foreach (var batch in v3InstallIds.Chunk(batchSize))
        {
            totalDeleted += await v2Context.StatRecord
                .Where(s => batch.AsEnumerable().Contains(s.InstallId))
                .ExecuteDeleteAsync();
        }

        return totalDeleted;
    }
}