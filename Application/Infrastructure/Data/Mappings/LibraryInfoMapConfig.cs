using Application.Domain.InstallationStatistics;
using MongoDB.Bson.Serialization;

namespace Application.Infrastructure.Data.Mappings
{
    public class LibraryInfoMapConfig
    {
        public static void ConfigureMap()
        {
            BsonClassMap.RegisterClassMap<LibraryInfo>(cm =>
            {
                cm.MapMember(c => c.Count);
                cm.MapMember(c => c.Type);
            });
        }
    }
}