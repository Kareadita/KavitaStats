using System;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Data;
using KavitaStats.Entities;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Services;

public class HistoricalSnapshotService(DataContextV3 context)
{
    public async Task TakeHistoricalSnapshot()
    {
        var today = DateTime.UtcNow.Date;
        
        // Check if we already have a snapshot for this week
        var existingSnapshot = await context.HistoricalSnapshot
            .AnyAsync(s => s.Date == today);
        
        if (existingSnapshot) return;
        
        var currentStats = await context.ServerStat
            .Where(s => !s.OptedOut)
            .Select(_ => new
            {
                UniqueInstalls = context.ServerStat.Count(s => !s.OptedOut),
                UniqueUsers = context.ServerStat
                    .Where(s => !s.OptedOut)
                    .SelectMany(s => s.Users)
                    .Count()
            })
            .FirstOrDefaultAsync();

        var uniqueInstalls = currentStats?.UniqueInstalls ?? 0;
        var uniqueUsers = currentStats?.UniqueUsers ?? 0;
        
        var previousSnapshot = await context.HistoricalSnapshot
            .OrderByDescending(s => s.Date)
            .FirstOrDefaultAsync();

        var snapshot = new HistoricalSnapshot
        {
            Date = today,
            UniqueInstalls = uniqueInstalls,
            UniqueUsers = uniqueUsers,
            DeltaInstalls = uniqueInstalls - (previousSnapshot?.UniqueInstalls ?? 0),
            DeltaUsers = uniqueUsers - (previousSnapshot?.UniqueUsers ?? 0)
        };

        context.HistoricalSnapshot.Add(snapshot);
        await context.SaveChangesAsync();
    }
}