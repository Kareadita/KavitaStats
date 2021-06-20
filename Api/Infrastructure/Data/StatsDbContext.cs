using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Common.Configurations;
using Api.Domain.InstallationStatistics;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api.Infrastructure.Data
{
    public class StatsDbContext
    {
        public IMongoDatabase Database { get; set; }
        private readonly List<Func<Task>> _commands;

        public MongoClient MongoClient { get; set; }
        private IClientSessionHandle Session { get; set; }

        public IMongoCollection<InstallationStatistics> Installations =>
            Database.GetCollection<InstallationStatistics>(nameof(InstallationStatistics));

        public StatsDbContext(IOptions<MongoDbOptions> options)
        {
            var dbSettings = options.Value ?? throw new ArgumentNullException(nameof(options));

            _commands = new List<Func<Task>>();

            MongoClient = new MongoClient(dbSettings.Connection);

            Database = MongoClient.GetDatabase(dbSettings.DatabaseName);
        }

        //Transaction does not work with standalone Mongo servers

        #region Unnit Of Work Code

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            using (Session = await MongoClient.StartSessionAsync(null, cancellationToken))
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync(cancellationToken);
            }

            return _commands.Count;
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        #endregion
    }
}