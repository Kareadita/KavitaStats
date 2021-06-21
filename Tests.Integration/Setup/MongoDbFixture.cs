using System;
using Application.Common.Configurations;
using Application.Infrastructure.Data;
using Microsoft.Extensions.Options;
using Xunit;

namespace Tests.Integration.Setup
{
    public class MongoDbFixture : IDisposable
    {
        public readonly MongoDbOptions MongoOptions;

        private bool _disposed;
        private readonly StatsDbContext _dbContext;

        public MongoDbFixture()
        {
            try
            {
                MongoOptions = new MongoDbOptions
                {
                    Connection = "mongodb://root:rootpassword@localhost:27017",
                    DatabaseName = "Test_DB"
                };

                _dbContext = new StatsDbContext(Options.Create(MongoOptions));
            }
            catch (Exception e)
            {
                _dbContext.MongoClient.DropDatabase(MongoOptions.DatabaseName);

                Console.WriteLine(e);
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // remove the temp db from the server once all tests are done
                _dbContext.MongoClient.DropDatabase(MongoOptions.DatabaseName);
            }

            _disposed = true;
        }
    }

    [CollectionDefinition("MongoDB")]
    public class DatabaseCollection : ICollectionFixture<MongoDbFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}