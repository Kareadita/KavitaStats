using Application.Domain.InstallationStatistics;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Application.Infrastructure.Data.Mappings
{
    public static class InstallationStatisticsMapConfig
    {
        public static void ConfigureMap()
        {
            BsonClassMap.RegisterClassMap<InstallationStatistics>(cm =>
            {
                cm.MapIdMember(c => c.Id).SetIdGenerator(CombGuidGenerator.Instance);
                cm.MapMember(c => c.InstallId);
                cm.MapProperty(c => c.LastUpdate);

                cm.MapProperty(c => c.UsageInfo);
                cm.MapProperty(c => c.ServerInfo);
                cm.MapProperty(c => c.ClientsInfo);
            });
        }
    }
}