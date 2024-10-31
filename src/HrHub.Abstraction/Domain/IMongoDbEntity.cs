using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HrHub.Abstraction.Domain
{
    public interface IMongoDbEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedAt { get; }

    }
}
