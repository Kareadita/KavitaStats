using System;
using System.Threading.Tasks;
using KavitaStats.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Tests;

public abstract class AbstractDbTest : IAsyncDisposable
{
    private SqliteConnection? _connection;
    private DataContextV3? _context;

    protected async Task<DataContextV3> CreateDatabase()
    {
        if (_connection != null)
        {
            await _context!.DisposeAsync();
            await _connection.DisposeAsync();
        }
        _connection = new SqliteConnection("Filename=:memory:");
        await _connection.OpenAsync();

        var contextOptions = new DbContextOptionsBuilder<DataContextV3>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;

        _context = new DataContextV3(contextOptions);

        await _context.Database.EnsureCreatedAsync();

        _context.ChangeTracker.Clear();

        return _context;
    }

    public async ValueTask DisposeAsync()
    {
        if (_context != null)
        {
            await _context.DisposeAsync();
        }

        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
