using HrHub.Abstraction.Domain;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HrHub.Abstraction.Data.EfCore.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        int Count(Expression<Func<TEntity, bool>> predicate = null);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default);


        // Db Insert
        void Add(TEntity entity);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Add(IEnumerable<TEntity> entities);
        Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        //Db Update
        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Update(List<TEntity> entities);
        Task UpdateAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

        // Db Delete
        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Delete(List<TEntity> entities);
        Task DeleteAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        bool Exists(Expression<Func<TEntity, bool>> predicate);

        TEntity Get(Expression<Func<TEntity, bool>> predicate,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                    Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                    Expression<Func<TEntity, TEntity>> selector = null);
        TResult Get<TResult>(Expression<Func<TEntity, bool>> predicate,
                             Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                             Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                             Expression<Func<TEntity, TResult>> selector = null);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
                                              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                              Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                              bool disableTracking = false,
                                              CancellationToken cancellationToken = default);
        Task<TResult> GetAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                        Expression<Func<TEntity, TResult>> selector = null,
                                        Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                        CancellationToken cancellationToken = default);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null,
                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                     bool disableTracking = false);
        IEnumerable<TResult> GetList<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                              Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                              Expression<Func<TEntity, TResult>> selector = null);
        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                CancellationToken cancellationToken = default);
        Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                         Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                         Expression<Func<TEntity, TResult>> selector = null,
                                                         CancellationToken cancellationToken = default);
        IEnumerable<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                          Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                          int skip = 0,
                                          int take = 20,
                                          bool disableTracking = false);
        IEnumerable<TResult> GetPagedList<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                   Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                   Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                   Expression<Func<TEntity, TResult>> selector = null,
                                                   int skip = 0,
                                                   int take = 20);
        Task<IEnumerable<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                     int skip = 0,
                                                     int take = 20,
                                                     CancellationToken cancellationToken = default);
        Task<IEnumerable<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                              Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                              Expression<Func<TEntity, TResult>> selector = null,
                                                              int skip = 0,
                                                              int take = 20,
                                                              CancellationToken cancellationToken = default);
        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate = null,
                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                     bool disableTracking = true,
                                     bool noLock = false);
        TEntity GetWithNoLock(Expression<Func<TEntity, bool>> predicate,
                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                              Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                              Expression<Func<TEntity, TEntity>> selector = null);
        TEntity GetWithNoLock(Expression<Func<TEntity, bool>> predicate,
                              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                              Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null);
        TResult GetWithNoLock<TResult>(Expression<Func<TEntity, bool>> predicate,
                                       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                       Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                       Expression<Func<TEntity, TResult>> selector = null);
        Task<TEntity> GetWithNoLockAsync(Expression<Func<TEntity, bool>> predicate,
                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                         Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                         bool disableTracking = false,
                                         CancellationToken cancellationToken = default);
        Task<TResult> GetWithNoLockAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                  Expression<Func<TEntity, TResult>> selector = null,
                                                  Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                  CancellationToken cancellationToken = default);
        IEnumerable<TEntity> GetListWithNoLock(Expression<Func<TEntity, bool>> predicate = null,
                                               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                               Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                               Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null);
        IEnumerable<TResult> GetListWithNoLock<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                        Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                        Expression<Func<TEntity, TResult>> selector = null);
        Task<IEnumerable<TEntity>> GetListWithNoLockAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                          Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                          CancellationToken cancellationToken = default);
        Task<IEnumerable<TResult>> GetListWithNoLockAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                                   Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                                   Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                                   Expression<Func<TEntity, TResult>> selector = null,
                                                                   CancellationToken cancellationToken = default);
        IEnumerable<TEntity> GetPagedListWithNoLock(Expression<Func<TEntity, bool>> predicate = null,
                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                    Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                    int skip = 0,
                                                    int take = 20,
                                                    bool disableTracking = false);
        IEnumerable<TResult> GetPagedListWithNoLock<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                             Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                             Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                             Expression<Func<TEntity, TResult>> selector = null,
                                                             int skip = 0,
                                                             int take = 20);
        Task<IEnumerable<TEntity>> GetPagedListWithNoLockAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                               Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                               Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                               int skip = 0,
                                                               int take = 20,
                                                               CancellationToken cancellationToken = default);
        Task AddOrUpdate(TEntity entity, CancellationToken cancellationToken = default);
        TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector);
        Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                        Expression<Func<TEntity, bool>> predicate = null,
                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                        Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                        CancellationToken cancellationToken = default);
    }
}
