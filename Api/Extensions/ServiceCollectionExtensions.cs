using Api.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<InfluxDbOptions>(configuration.GetSection(nameof(InfluxDbOptions)));

            return services;
        }
    }
}