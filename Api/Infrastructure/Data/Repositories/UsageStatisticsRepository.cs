using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Configurations;
using Api.Domain.UsageStatistics;
using Api.Domain.UsageStatistics.Contracts;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;


namespace Api.Infrastructure.Data.Repositories
{
    public class UsageStatisticsRepository : IUsageStatisticsRepository
    {
        private readonly InfluxDBClient _dbClient;
        private readonly InfluxDbOptions _options;

        public UsageStatisticsRepository(IOptions<InfluxDbOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _dbClient = InfluxDBClientFactory.Create(_options.Url, _options.Token.ToCharArray());
        }

        public Task Add(UsageStatistics usageStatistics)
        {
            var point = Point
                .Measurement("all")
                .Tag(nameof(UsageStatistics.ServerInfo), JsonSerializer.Serialize(usageStatistics.ServerInfo))
                .Field(nameof(UsageStatistics.UsersCount), usageStatistics.UsersCount)
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            using var writeApi = _dbClient.GetWriteApi();

            writeApi.WritePoint(_options.Bucket, _options.Org, point);

            return Task.CompletedTask;
        }

        public Task Add(ServerInfo serverInfo)
        {
            var point = Point
                .Measurement("server")
                .Tag(nameof(UsageStatistics.ServerInfo), JsonSerializer.Serialize(serverInfo))
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            using var writeApi = _dbClient.GetWriteApi();

            writeApi.WritePoint(_options.Bucket, _options.Org, point);

            return Task.CompletedTask;
        }

        public Task Add(ClientInfo clientInfo)
        {
            var point = Point
                .Measurement("client")
                .Tag(nameof(UsageStatistics.ClientsInfo), JsonSerializer.Serialize(clientInfo))
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            using var writeApi = _dbClient.GetWriteApi();

            writeApi.WritePoint(_options.Bucket, _options.Org, point);

            return Task.CompletedTask;
        }
    }
}