using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HrHub.Abstraction.Domain;

namespace HrHub.Abstraction.Data.MongoDb
{
    public interface IMongoRepository<TKey> where TKey : IMongoDbEntity
    {
        IQueryable<TKey> AsQueryable();

        IEnumerable<TKey> FilterBy(
            Expression<Func<TKey, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TKey, bool>> filterExpression,
            Expression<Func<TKey, TProjected>> projectionExpression);

        TKey FindOne(Expression<Func<TKey, bool>> filterExpression);

        Task<TKey> FindOneAsync(Expression<Func<TKey, bool>> filterExpression);

        TKey FindById(string id);

        Task<TKey> FindByIdAsync(string id);

        void InsertOne(TKey document);

        Task InsertOneAsync(TKey document);

        void InsertMany(ICollection<TKey> documents);

        Task InsertManyAsync(ICollection<TKey> documents);

        void ReplaceOne(TKey document);

        Task ReplaceOneAsync(TKey document);

        void DeleteOne(Expression<Func<TKey, bool>> filterExpression);

        Task DeleteOneAsync(Expression<Func<TKey, bool>> filterExpression);

        void DeleteById(string id);

        Task DeleteByIdAsync(string id);

        void DeleteMany(Expression<Func<TKey, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<TKey, bool>> filterExpression);
    }
}
