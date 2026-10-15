using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KavitaStats.Entities.V3;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Data.Repository;

public interface IServerStatRepository
{
    Task<int> DeleteWhereAsync(Expression<Func<ServerStat, bool>> predicate);
}

public class ServerStatRepository(DataContextV3 context) : IServerStatRepository
{
    private const int BatchSize = 500;

    /// <returns>Number of ServerStat rows deleted</returns>
    public async Task<int> DeleteWhereAsync(Expression<Func<ServerStat, bool>> predicate)
    {
        var ids = await context.ServerStat
            .Where(predicate)
            .Select(s => s.Id)
            .ToListAsync();

        var totalDeleted = 0;
        foreach (var batch in ids.Chunk(BatchSize))
        {
            var batchList = batch.ToList();

            await context.LibraryStat
                .Where(l => batchList.Contains(l.ServerStatId))
                .ExecuteDeleteAsync();

            await context.RelationshipStat
                .Where(r => batchList.Contains(r.ServerStatId))
                .ExecuteDeleteAsync();

            await context.UserStat
                .Where(u => batchList.Contains(u.ServerStatId))
                .ExecuteDeleteAsync();

            totalDeleted += await context.ServerStat
                .Where(s => batchList.Contains(s.Id))
                .ExecuteDeleteAsync();
        }

        return totalDeleted;
    }
}
