using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace AdventureDayRunner.Shared
{
    public static class BsonSerializerSettings
    {
        public static void Configure()
        {
            BsonSerializer.RegisterSerializer(new EnumSerializer<AdventureDayPhase>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<AdventureDayRunnerStatus>(BsonType.String));
            BsonSerializer.RegisterSerializer(new EnumSerializer<PlayerType>(BsonType.String));
        }
    }
}