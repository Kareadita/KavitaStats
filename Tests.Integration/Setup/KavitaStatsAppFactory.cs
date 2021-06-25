using System;
using System.Collections.Generic;
using System.Net.Http;
using Application;
using Application.Common.Configurations;
using Application.Common.Constants;
using Application.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Setup
{
    public class KavitaStatsAppFactory : WebApplicationFactory<Startup>
    {
        private readonly MongoDbFixture _dbFixture;

        public KavitaStatsAppFactory(MongoDbFixture dbFixture)
        {
            _dbFixture = dbFixture;
        }

        private const string TestApiKey = "test-api-key";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(AppConstants.ApikeyName, TestApiKey),
                    new KeyValuePair<string, string>(
                        $"{nameof(MongoDbOptions)}:Connection", _dbFixture.MongoOptions.Connection),
                    new KeyValuePair<string, string>(
                        $"{nameof(MongoDbOptions)}:DatabaseName", _dbFixture.MongoOptions.DatabaseName),
                });
            });
        }

        public HttpClient CreateAppClient(WebApplicationFactoryClientOptions options = null)
        {
            var opts = options ?? new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri($"http://localhost:5003")
            };

            var client = base.CreateClient(opts);

            client.DefaultRequestHeaders.Add(AppConstants.AuthHeaderKey, TestApiKey);

            return client;
        }

        public StatsDbContext GetDbContext() => Services.GetRequiredService<StatsDbContext>();
    }
}