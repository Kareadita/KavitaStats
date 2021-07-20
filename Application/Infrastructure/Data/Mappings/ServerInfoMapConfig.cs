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
                cm.MapMember(c => c.Culture);
                cm.MapMember(c => c.BuildBranch);
                cm.MapMember(c => c.KavitaVersion);
                cm.MapMember(c => c.DotNetVersion);
                cm.MapMember(c => c.RunTimeVersion);
                cm.MapMember(c => c.IsDocker);
                cm.MapMember(c => c.NumOfCores);
            });
        }
    }
}