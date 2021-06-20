using Application.Domain.InstallationStatistics;
using MongoDB.Bson.Serialization;

namespace Application.Infrastructure.Data.Mappings
{
    public static class ServerInfoMapConfig
    {
        public static void ConfigureMap()
        {
            BsonClassMap.RegisterClassMap<ServerInfo>(cm =>
            {
                cm.MapMember(c => c.Os);
                cm.MapIdMember(c => c.Culture);
                cm.MapProperty(c => c.BuildBranch);
                cm.MapProperty(c => c.KavitaVersion);
                cm.MapProperty(c => c.DotNetVersion);
                cm.MapProperty(c => c.RunTimeVersion);
            });
        }
    }
}