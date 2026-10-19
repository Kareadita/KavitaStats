using System;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Data;
using KavitaStats.Entities;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Services;

public class HistoricalSnapshotService(DataContextV3 context, IUiStatsCacheService cacheService)
{
    public async Task TakeHistoricalSnapshot()
    {
        var now = DateTime.UtcNow;
        var today = now.Date;
        var activeCutoff = now.AddDays(-10);
        var newCutoff = now.AddDays(-7);
        var churnStart = now.AddDays(-37);
        var churnEnd = now.AddDays(-30);

        var uniqueInstalls = await context.ServerStat.CountAsync(s => !s.OptedOut);
        var uniqueUsers = await context.ServerStat
            .Where(s => !s.OptedOut)
            .SelectMany(s => s.Users)
            .CountAsync();

        var previousSnapshot = await context.HistoricalSnapshot
            .OrderByDescending(s => s.Date)
            .FirstOrDefaultAsync();

        var activeInstalls = context.ServerStat.Where(s => !s.OptedOut && s.LastModified >= activeCutoff);

        var snapshot = new HistoricalSnapshot
        {
            Date = today,
            UniqueInstalls = uniqueInstalls,
            UniqueUsers = uniqueUsers,
            DeltaInstalls = uniqueInstalls - (previousSnapshot?.UniqueInstalls ?? 0),
            DeltaUsers = uniqueUsers - (previousSnapshot?.UniqueUsers ?? 0),
            ActiveInstalls = await cacheService.GetActiveInstallsAsync(),
            NewInstalls = await context.InstallLifecycle
                .CountAsync(l => !l.Seeded && l.FirstSeen >= newCutoff
                                 && !context.ServerStat.Any(s => s.InstallId == l.InstallId && s.OptedOut)),
            ChurnedInstalls = await context.ServerStat
                .CountAsync(s => !s.OptedOut && s.LastModified >= churnStart && s.LastModified < churnEnd),
            OptedOutInstalls = await context.ServerStat.CountAsync(s => s.OptedOut),
            ActiveUsers = await activeInstalls.SelectMany(s => s.Users).CountAsync(),
            InstallsWithContent = await activeInstalls.CountAsync(s => s.TotalSeries > 0)
        };

        await using var transaction = await context.Database.BeginTransactionAsync();

        context.HistoricalSnapshot.Add(snapshot);
        await context.SaveChangesAsync();

        var versionCounts = await activeInstalls
            .GroupBy(s => s.KavitaVersion)
            .Select(g => new SnapshotVersionCount
            {
                HistoricalSnapshotId = snapshot.Id,
                KavitaVersion = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        context.SnapshotVersionCount.AddRange(versionCounts);
        await context.SaveChangesAsync();

        await transaction.CommitAsync();
    }
}
