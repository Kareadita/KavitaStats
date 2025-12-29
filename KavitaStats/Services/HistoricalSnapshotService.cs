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

        var uniqueInstalls = await context.ServerStat.CountAsync(s => !s.OptedOut);
        var uniqueUsers = await context.ServerStat
            .Where(s => !s.OptedOut)
            .SelectMany(s => s.Users)
            .CountAsync();

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