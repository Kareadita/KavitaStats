using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KavitaStats.Data;
using KavitaStats.Entities;
using KavitaStats.Entities.V2;
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
    }

    public async Task CleanupRecords()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var services = scope.ServiceProvider;
        var v2Context = services.GetRequiredService<DataContext>();
        var v3Context = services.GetRequiredService<DataContextV3>();

        var oneYearAgo = DateTime.UtcNow.AddYears(-1);

        var upgradedRecordsDeleted = await RemoveV2RecordsThatMigratedToV3(v2Context, v3Context);
        var staleV2RecordsDeleted = await RemoveStaleV2Records(v2Context, oneYearAgo);
        var staleV3RecordsDeleted = await RemoveStaleV3Records(v3Context, oneYearAgo);

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

    private static async Task<int> RemoveStaleV2Records(DataContext v2Context, DateTime cutoff)
    {
        const int batchSize = 500;

        var staleRecordIds = await v2Context.StatRecord
            .Where(s => s.LastModified < cutoff)
            .Select(s => s.Id)
            .ToListAsync();

        if (staleRecordIds.Count == 0) return 0;

        var totalDeleted = 0;
        foreach (var batch in staleRecordIds.Chunk(batchSize))
        {
            var batchList = batch.ToList();

            // Delete children first
            await v2Context.Set<Color>()
                .Where(c => batchList.Contains(c.StatRecordId))
                .ExecuteDeleteAsync();

            await v2Context.Set<PageSplit>()
                .Where(p => batchList.Contains(p.StatRecordId))
                .ExecuteDeleteAsync();

            await v2Context.Set<MangaReaderLayoutMode>()
                .Where(m => batchList.Contains(m.StatRecordId))
                .ExecuteDeleteAsync();

            // FileFormat uses a shadow property for the FK
            await v2Context.Set<FileFormat>()
                .Where(f => batchList.Contains(EF.Property<int>(f, "StatRecordId")))
                .ExecuteDeleteAsync();

            // Delete parent records
            totalDeleted += await v2Context.StatRecord
                .Where(s => batchList.Contains(s.Id))
                .ExecuteDeleteAsync();
        }

        return totalDeleted;
    }

    private static async Task<int> RemoveStaleV3Records(DataContextV3 v3Context, DateTime cutoff)
    {
        const int batchSize = 500;

        var staleRecordIds = await v3Context.ServerStat
            .Where(s => s.LastModified < cutoff)
            .Select(s => s.Id)
            .ToListAsync();

        if (staleRecordIds.Count == 0) return 0;

        var totalDeleted = 0;
        foreach (var batch in staleRecordIds.Chunk(batchSize))
        {
            var batchList = batch.ToList();

            // Delete children first
            await v3Context.LibraryStat
                .Where(l => batchList.Contains(l.ServerStatId))
                .ExecuteDeleteAsync();

            await v3Context.RelationshipStat
                .Where(r => batchList.Contains(r.ServerStatId))
                .ExecuteDeleteAsync();

            await v3Context.UserStat
                .Where(u => batchList.Contains(u.ServerStatId))
                .ExecuteDeleteAsync();

            // Delete parent records
            totalDeleted += await v3Context.ServerStat
                .Where(s => batchList.Contains(s.Id))
                .ExecuteDeleteAsync();
        }

        return totalDeleted;
    }
}