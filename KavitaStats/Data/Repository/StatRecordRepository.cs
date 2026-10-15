using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KavitaStats.Entities;
using KavitaStats.Entities.V2;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Data.Repository;

public interface IStatRecordRepository
{
    Task<int> DeleteWhereAsync(Expression<Func<StatRecord, bool>> predicate);
}

public class StatRecordRepository(DataContext context) : IStatRecordRepository
{
    private const int BatchSize = 500;

    /// <returns>Number of StatRecord rows deleted</returns>
    public async Task<int> DeleteWhereAsync(Expression<Func<StatRecord, bool>> predicate)
    {
        var ids = await context.StatRecord
            .Where(predicate)
            .Select(s => s.Id)
            .ToListAsync();

        var totalDeleted = 0;
        foreach (var batch in ids.Chunk(BatchSize))
        {
            var batchList = batch.ToList();

            await context.Set<Color>()
                .Where(c => batchList.Contains(c.StatRecordId))
                .ExecuteDeleteAsync();

            await context.Set<PageSplit>()
                .Where(p => batchList.Contains(p.StatRecordId))
                .ExecuteDeleteAsync();

            await context.Set<MangaReaderLayoutMode>()
                .Where(m => batchList.Contains(m.StatRecordId))
                .ExecuteDeleteAsync();

            // FileFormat uses a shadow property for the FK
            await context.Set<FileFormat>()
                .Where(f => batchList.Contains(EF.Property<int>(f, "StatRecordId")))
                .ExecuteDeleteAsync();

            totalDeleted += await context.StatRecord
                .Where(s => batchList.Contains(s.Id))
                .ExecuteDeleteAsync();
        }

        return totalDeleted;
    }
}
