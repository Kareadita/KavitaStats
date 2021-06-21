using Application.Domain.InstallationStatistics;
using MongoDB.Bson.Serialization;

namespace Application.Infrastructure.Data.Mappings
{
    public static class ClientInfoMapConfig
    {
        public static void ConfigureMap()
        {
            BsonClassMap.RegisterClassMap<ClientInfo>(cm =>
            {
                cm.MapProperty(c => c.ScreenResolution);
                cm.MapProperty(c => c.KavitaUiVersion);
                cm.MapProperty(c => c.CollectedAt);
                cm.MapMember(c => c.PlatformType);
                cm.MapProperty(c => c.Browser);
                cm.MapProperty(c => c.Os);
            });
        }
    }
}