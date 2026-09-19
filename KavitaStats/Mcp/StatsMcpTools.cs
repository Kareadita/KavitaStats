using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KavitaStats.Data;
using KavitaStats.Entities;
using KavitaStats.Entities.Enum;
using KavitaStats.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace KavitaStats.Mcp;

public record StatsSchema(string Notes, IReadOnlyList<string> V3Tables, IReadOnlyList<string> V2Tables,
    IReadOnlyDictionary<string, IReadOnlyDictionary<int, string>> Enums);

public record StatsOverview(int ActiveInstalls, int TotalInstalls, int V3ReportingInstalls, int V3OptedOutInstalls,
    int V3Users, IReadOnlyList<HistoricalSnapshot> RecentWeeklySnapshots);

[McpServerToolType]
public class StatsMcpTools(ReadOnlySqlRunner sqlRunner, DataContextV3 dataContextV3, IUiStatsCacheService cacheService)
{
    private const string SchemaNotes = """
        stats-v3.db ("v3") is the current data. stats.db ("v2") is legacy data from Kavita versions before v3 stats.

        v3 tables:
        - ServerStat: one row per Kavita install, unique on InstallId. The row is overwritten each time the install reports in, so it is the latest state, not history.
        - UserStat, LibraryStat, RelationshipStat: child rows of ServerStat via ServerStatId, replaced on every report. UserStat is one row per user account on that install.
        - HistoricalSnapshot: weekly totals of installs and users, the only time series.

        Conventions:
        - LastModified on ServerStat is the last time the install reported. The kavitareader.com site counts an install as "active" when LastModified is within the last 10 days.
        - Exclude ServerStat rows with OptedOut = 1 unless the question is about opt-outs.
        - Dates are TEXT, e.g. '2026-01-31 14:05:00'. Created and LastModified are UTC. Compare with datetime('now', '-30 days').
        - Enum columns are stored as integers, see Enums for the mapping.
        - UserStat.Roles, UserStat.DevicePlatforms and LibraryStat.FileTypes are JSON arrays. Query them with json_each, e.g. SELECT COUNT(*) FROM UserStat, json_each(UserStat.DevicePlatforms) WHERE json_each.value = 2
        - An install that upgraded may exist in both v2 StatRecord and v3 ServerStat. Deduplicate on InstallId when combining.
        - sqlite_master, sqlite_schema and PRAGMA in any form are blocked. This tool gives you the full structure, do not try to introspect it in SQL.
        """;

    [McpServerTool(Name = "get_schema", ReadOnly = true)]
    [Description("Returns the table definitions of both stats databases, the integer meaning of every enum column, and notes on how to interpret the data. Call this before writing any query.")]
    public async Task<StatsSchema> GetSchema(CancellationToken cancellationToken)
    {
        return new StatsSchema(
            SchemaNotes,
            await sqlRunner.GetTableDefinitionsAsync(StatsDatabase.V3, cancellationToken),
            await sqlRunner.GetTableDefinitionsAsync(StatsDatabase.V2, cancellationToken),
            GetEnumMappings());
    }

    [McpServerTool(Name = "run_query", ReadOnly = true)]
    [Description("Runs a read-only SQLite SELECT against a stats database and returns up to 500 rows. Aggregate in SQL rather than pulling raw rows.")]
    public async Task<SqlQueryResult> RunQuery(
        [Description("Which database to query: V3 (current) or V2 (legacy)")] StatsDatabase database,
        [Description("A single SQLite SELECT statement")] string sql,
        CancellationToken cancellationToken)
    {
        try
        {
            return await sqlRunner.QueryAsync(database, sql, cancellationToken);
        }
        catch (SqliteException ex)
        {
            throw new McpException($"SQLite error: {ex.Message}");
        }
    }

    [McpServerTool(Name = "get_overview", ReadOnly = true)]
    [Description("Headline numbers: active installs (reported in the last 10 days), all-time installs across v2 and v3, v3 install and user counts, and the last 12 weekly snapshots.")]
    public async Task<StatsOverview> GetOverview(CancellationToken cancellationToken)
    {
        var snapshots = await dataContextV3.HistoricalSnapshot
            .OrderByDescending(s => s.Date)
            .Take(12)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new StatsOverview(
            await cacheService.GetActiveInstallsAsync(),
            await cacheService.GetTotalInstallsAsync(),
            await dataContextV3.ServerStat.CountAsync(s => !s.OptedOut, cancellationToken),
            await dataContextV3.ServerStat.CountAsync(s => s.OptedOut, cancellationToken),
            await dataContextV3.UserStat.CountAsync(u => !u.ServerStat.OptedOut, cancellationToken),
            snapshots);
    }

    private static IReadOnlyDictionary<string, IReadOnlyDictionary<int, string>> GetEnumMappings()
    {
        return typeof(LibraryType).Assembly.GetTypes()
            .Where(t => t.IsEnum && t.Namespace == typeof(LibraryType).Namespace)
            .OrderBy(t => t.Name)
            .ToDictionary(
                t => t.Name,
                t => (IReadOnlyDictionary<int, string>) Enum.GetValues(t).Cast<object>()
                    .ToDictionary(Convert.ToInt32, v => v.ToString()));
    }
}
