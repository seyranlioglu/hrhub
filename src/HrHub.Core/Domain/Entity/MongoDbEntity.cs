using HrHub.Abstraction.Domain;
using MongoDB.Bson;

namespace HrHub.Core.Domain.Entity
{
    public abstract class MongoDbEntity : IMongoDbEntity
    {
        public MongoDbEntity()
        {
            Id = ObjectId.GenerateNewId();
        }
        public ObjectId Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
