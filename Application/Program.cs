using System;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sentry;

namespace Application
{
    public class Program
    {
        private static int _httpPort;
        public static void Main(string[] args)
        {
            _httpPort = GetPort(GetAppSettingFilename());
            CreateHostBuilder(args).Build().Run();
        }

        private static readonly string Environment
            = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        private static IConfiguration Configuration { get; set; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment}.json", optional: true)
            .AddJsonFile($"appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel((opts) =>
                    {
                        opts.ListenAnyIP(_httpPort, options =>
                        {
                            options.Protocols = HttpProtocols.Http1AndHttp2;
                        });
                    });

                    webBuilder.UseSentry(options =>
                    {
                        options.Dsn = "https://8a83109b985c414d890d2335dd9944c7@o641015.ingest.sentry.io/5824746";
                        options.MaxBreadcrumbs = 200;
                        options.AttachStacktrace = true;
                        options.Debug = false;
                        options.SendDefaultPii = false;
                        options.DiagnosticLevel = SentryLevel.Debug;
                        options.ShutdownTimeout = TimeSpan.FromSeconds(5);
                        options.Release = Environment;
                    });

                    webBuilder
                        .UseConfiguration(Configuration)
                        .UseStartup<Startup>();
                });
        
        private static string GetAppSettingFilename()
        {
            var isDevelopment = Environment == Environments.Development;
            return "appsettings" + (isDevelopment ? ".Development" : "") + ".json";
        }
        
        private static int GetPort(string filePath)
        {
            const int defaultPort = 5001;

            try {
                var json = File.ReadAllText(filePath);
                var jsonObj = JsonSerializer.Deserialize<dynamic>(json);
                const string key = "Port";
                
                if (jsonObj.TryGetProperty(key, out JsonElement tokenElement))
                {
                    return tokenElement.GetInt32();
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Error writing app settings: " + ex.Message);
            }

            return defaultPort;
        }
    }
}