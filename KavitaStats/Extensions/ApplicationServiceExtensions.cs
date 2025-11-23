using KavitaStats.Data;
using KavitaStats.Data.Helpers;
using KavitaStats.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NReco.Logging.File;

namespace KavitaStats.Extensions;

public static class ApplicationServiceExtensions
{
    extension(IServiceCollection services)
    {
        public void AddApplicationServices(IConfiguration config, IWebHostEnvironment env)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITaskScheduler, TaskScheduler>();

            services.AddLogging(config);
            services.AddSqLite(config, env);
            services.AddSignalR();
        }

        private void AddSqLite(IConfiguration config,
            IHostEnvironment env)
        {
            services.AddDbContextPool<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(env.IsDevelopment());
            });
        
            services.AddDbContextPool<DataContextV3>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnectionV3"));
                options.EnableSensitiveDataLogging(env.IsDevelopment());
            });
        }

        private void AddLogging(IConfiguration config)
        {
            services.AddLogging(loggingBuilder =>
            {
                var loggingSection = config.GetSection("Logging");
                loggingBuilder.AddFile(loggingSection);
            });
        }
    }
}