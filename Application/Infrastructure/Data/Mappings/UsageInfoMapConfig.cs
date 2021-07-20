using Application.Domain.InstallationStatistics;
using MongoDB.Bson.Serialization;

namespace Application.Infrastructure.Data.Mappings
{
    public class UsageInfoMapConfig
    {
        public static void ConfigureMap()
        {
            BsonClassMap.RegisterClassMap<UsageInfo>(cm =>
            {
                cm.MapMember(c => c.UsersCount);
                cm.MapProperty(c => c.FileTypes);
                cm.MapProperty(c => c.LibraryTypesCreated);
            });
        }
    }
}