using Application.Infrastructure.Data.Mappings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Application.Infrastructure.Data
{
    public static class MongoConfigurations
    {
        public static void ConfigureDbSettings()
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            RegisterConventions();
            RegisterMappings();
        }

        private static void RegisterMappings()
        {
            InstallationStatisticsMapConfig.ConfigureMap();
            ServerInfoMapConfig.ConfigureMap();
            ClientInfoMapConfig.ConfigureMap();
            UsageInfoMapConfig.ConfigureMap();
        }

        private static void RegisterConventions()
        {
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(true),
            };

            ConventionRegistry.Register("KavitaStats Conventions", pack, t => true);
        }
    }
}