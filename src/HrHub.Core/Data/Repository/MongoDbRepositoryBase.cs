using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Data.MongoDb;
using HrHub.Abstraction.Domain;
using HrHub.Abstraction.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace HrHub.Core.Data.Repository
{
    public class MongoRepository<TKey> : IMongoRepository<TKey>
           where TKey : IMongoDbEntity
    {
        private readonly IMongoCollection<TKey> _collection;

        public MongoRepository(IMongoDbSettings settings)
        {
            var mongoSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
            mongoSettings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.Ssl2
            };
            var database = new MongoClient(mongoSettings).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TKey>(GetCollectionName(typeof(TKey)));
        }

        private protected string? GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public virtual IQueryable<TKey> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TKey> FilterBy(
            Expression<Func<TKey, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TKey, bool>> filterExpression,
            Expression<Func<TKey, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TKey FindOne(Expression<Func<TKey, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TKey> FindOneAsync(Expression<Func<TKey, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TKey FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TKey>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TKey> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TKey>.Filter.Eq(doc => doc.Id, objectId);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }


        public virtual void InsertOne(TKey document)
        {
            _collection.InsertOne(document);
        }

        public virtual async Task InsertOneAsync(TKey document)
        {
            await _collection.InsertOneAsync(document);
        }

        public void InsertMany(ICollection<TKey> documents)
        {
            _collection.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<TKey> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TKey document)
        {
            var filter = Builders<TKey>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TKey document)
        {
            var filter = Builders<TKey>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TKey, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TKey, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TKey>.Filter.Eq(doc => doc.Id, objectId);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TKey>.Filter.Eq(doc => doc.Id, objectId);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TKey, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TKey, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }
    }
}
