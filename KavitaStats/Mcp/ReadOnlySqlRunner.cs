using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using SQLitePCL;

namespace KavitaStats.Mcp;

public record SqlQueryResult(IReadOnlyList<string> Columns, IReadOnlyList<object[]> Rows, bool Truncated);

public enum StatsDatabase
{
    V3,
    V2
}

/// <summary>
/// Runs arbitrary SQL from the MCP client against a read-only connection that SQLite itself restricts to SELECT
/// </summary>
public class ReadOnlySqlRunner(IConfiguration config)
{
    public const int MaxRows = 500;
    private static readonly TimeSpan MaxDuration = TimeSpan.FromSeconds(20);
    private const int MaxValueLength = 4 * 1024 * 1024;
    private const long MaxResultBytes = 8L * 1024 * 1024;
    private static readonly string[] HiddenTablePrefixes = ["AspNet", "__EFMigrations", "sqlite_"];

    private static readonly int[] AllowedActions =
    [
        raw.SQLITE_SELECT,
        raw.SQLITE_READ,
        raw.SQLITE_FUNCTION,
        raw.SQLITE_RECURSIVE
    ];

    /// <summary>
    /// load_extension reaches outside the database file, fts3_tokenizer takes a raw pointer in its two argument form
    /// </summary>
    private static readonly string[] DeniedFunctions = ["load_extension", "fts3_tokenizer"];

    private static bool IsHiddenTable(string tableName) =>
        HiddenTablePrefixes.Any(p => tableName.StartsWith(p, StringComparison.OrdinalIgnoreCase));

    public async Task<SqlQueryResult> QueryAsync(StatsDatabase database, string sql, CancellationToken cancellationToken)
    {
        await using var connection = await OpenAsync(database, cancellationToken);

        var stopwatch = Stopwatch.StartNew();
        raw.sqlite3_progress_handler(connection.Handle, 10_000, _ => stopwatch.Elapsed > MaxDuration ? 1 : 0, null);

        // The progress handler only interrupts between opcodes, so a single zeroblob() allocation would slip past it
        raw.sqlite3_limit(connection.Handle, raw.SQLITE_LIMIT_LENGTH, MaxValueLength);

        if (raw.sqlite3_set_authorizer(connection.Handle, (strdelegate_authorizer) Authorize, null) != raw.SQLITE_OK)
        {
            throw new InvalidOperationException("Could not install the SQLite authorizer, refusing to run the query");
        }

        await using var command = connection.CreateCommand();
        command.CommandText = sql;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
        var rows = new List<object[]>();
        var truncated = false;
        var bytes = 0L;

        while (await reader.ReadAsync(cancellationToken))
        {
            // MaxValueLength caps a single value, this caps 500 rows of merely large ones
            if (rows.Count == MaxRows || bytes > MaxResultBytes)
            {
                truncated = true;
                break;
            }

            var row = new object[reader.FieldCount];
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                row[i] = value is byte[] ? "<blob>" : value;
                if (row[i] is string text) bytes += text.Length * 2L;
            }
            rows.Add(row);
        }

        return new SqlQueryResult(columns, rows, truncated);
    }

    /// <summary>
    /// Returns the CREATE TABLE statements for every table the query tool may read
    /// </summary>
    public async Task<IReadOnlyList<string>> GetTableDefinitionsAsync(StatsDatabase database, CancellationToken cancellationToken)
    {
        await using var connection = await OpenAsync(database, cancellationToken);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT name, sql FROM sqlite_master WHERE type = 'table' ORDER BY name";

        var definitions = new List<string>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            if (IsHiddenTable(reader.GetString(0))) continue;
            definitions.Add(reader.GetString(1));
        }

        return definitions;
    }

    private async Task<SqliteConnection> OpenAsync(StatsDatabase database, CancellationToken cancellationToken)
    {
        var connectionName = database == StatsDatabase.V3 ? "DefaultConnectionV3" : "DefaultConnection";
        var builder = new SqliteConnectionStringBuilder(config.GetConnectionString(connectionName))
        {
            Mode = SqliteOpenMode.ReadOnly,
            Pooling = false
        };

        var connection = new SqliteConnection(builder.ToString());
        await connection.OpenAsync(cancellationToken);
        return connection;
    }

    private static int Authorize(object userData, int actionCode, string param0, string param1, string dbName, string triggerOrView)
    {
        if (!AllowedActions.Contains(actionCode)) return raw.SQLITE_DENY;

        // SQLITE_READ passes the table in param0, SQLITE_FUNCTION passes the function name in param1 and leaves param0 null
        if (actionCode == raw.SQLITE_READ && param0 != null && IsHiddenTable(param0)) return raw.SQLITE_DENY;
        if (actionCode == raw.SQLITE_FUNCTION && param1 != null &&
            DeniedFunctions.Contains(param1, StringComparer.OrdinalIgnoreCase))
        {
            return raw.SQLITE_DENY;
        }

        return raw.SQLITE_OK;
    }
}
