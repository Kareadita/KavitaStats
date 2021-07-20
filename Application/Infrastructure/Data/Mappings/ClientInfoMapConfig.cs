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
                cm.MapMember(c => c.ScreenResolution);
                cm.MapMember(c => c.KavitaUiVersion);
                cm.MapMember(c => c.CollectedAt);
                cm.MapMember(c => c.PlatformType);
                cm.MapMember(c => c.Browser);
                cm.MapMember(c => c.Os);
                cm.MapMember(c => c.UsingDarkTheme);
            });
        }
    }
}