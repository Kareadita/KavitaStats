using System;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace KavitaStats.Services;

public interface IUiStatsCacheService
{
    Task<int> GetActiveInstallsAsync();
    Task<int> GetTotalInstallsAsync();
}

public class UiStatsCacheService(DataContext dataContext, DataContextV3 dataContextV3, IMemoryCache cache)
    : IUiStatsCacheService
{
    private const string ActiveInstallsCacheKey = "ui:active-installs";
    private const string TotalInstallsCacheKey = "ui:total-installs";
    private static readonly TimeSpan ActiveInstallsCacheDuration = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan TotalInstallsCacheDuration = TimeSpan.FromHours(1);

    public async Task<int> GetActiveInstallsAsync()
    {
        if (cache.TryGetValue(ActiveInstallsCacheKey, out int cached))
            return cached;

        var cutoff = DateTime.Now.Subtract(TimeSpan.FromDays(10));

        var v2InstallIds = await dataContext.StatRecord
            .Where(s => s.LastModified >= cutoff)
            .Select(s => s.InstallId)
            .Distinct()
            .ToListAsync();

        var v2Set = v2InstallIds.ToHashSet();

        var v3UniqueCount = await dataContextV3.ServerStat
            .Where(s => s.LastModified >= cutoff)
            .Select(s => s.InstallId)
            .Distinct()
            .ToListAsync()
            .ContinueWith(t => t.Result.Count(id => !v2Set.Contains(id)));

        var result = v2InstallIds.Count + v3UniqueCount;

        cache.Set(ActiveInstallsCacheKey, result, ActiveInstallsCacheDuration);

        return result;
    }

    public async Task<int> GetTotalInstallsAsync()
    {
        if (cache.TryGetValue(TotalInstallsCacheKey, out int cached))
            return cached;

        var v2Count = await dataContext.StatRecord
            .Select(s => s.InstallId)
            .Distinct()
            .CountAsync();

        var v3InstallIds = await dataContextV3.ServerStat
            .Select(s => s.InstallId)
            .Distinct()
            .ToListAsync();

        var overlapCount = 0;
        const int batchSize = 500;

        foreach (var batch in v3InstallIds.Chunk(batchSize))
        {
            overlapCount += await dataContext.StatRecord
                .Where(s => batch.Contains(s.InstallId))
                .Select(s => s.InstallId)
                .Distinct()
                .CountAsync();
        }

        var result = v2Count + v3InstallIds.Count - overlapCount;

        cache.Set(TotalInstallsCacheKey, result, TotalInstallsCacheDuration);

        return result;
    }
}
