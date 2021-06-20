using Api.Common.Configurations;
using Api.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbOptions>(configuration.GetSection(nameof(MongoDbOptions)));
            
            return services;
        }

        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<StatsDbContext>();
            MongoConfigurations.ConfigureDbSettings();

            return services;
        }
    }
}