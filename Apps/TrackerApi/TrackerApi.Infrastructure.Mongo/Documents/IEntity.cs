using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrackerApi.Infrastructure.Mongo.Documents
{
    public interface IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
