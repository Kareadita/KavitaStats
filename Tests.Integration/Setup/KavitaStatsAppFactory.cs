using System;
using System.Collections.Generic;
using System.Net.Http;
using Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace Tests.Integration.Setup
{
    public class KavitaStatsAppFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                
            });
        }
        
        public HttpClient CreateAppClient(WebApplicationFactoryClientOptions options = null)
        {
            var opts = options ?? new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri($"https://localhost:5002")
            };

            return base.CreateClient(opts);
        }
    }
}