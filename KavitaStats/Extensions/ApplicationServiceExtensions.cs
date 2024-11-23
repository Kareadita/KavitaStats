using KavitaStats.Data;
using KavitaStats.Data.Helpers;
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
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddLogging(config);
        services.AddSqLite(config, env);
        services.AddSignalR();
    }

    private static void AddSqLite(this IServiceCollection services, IConfiguration config,
        IHostEnvironment env)
    {
        services.AddDbContextPool<DataContext>(options =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            options.EnableSensitiveDataLogging(env.IsDevelopment());
        });
    }

    private static void AddLogging(this IServiceCollection services, IConfiguration config)
    {
        services.AddLogging(loggingBuilder =>
        {
            var loggingSection = config.GetSection("Logging");
            loggingBuilder.AddFile(loggingSection);
        });
    }
}