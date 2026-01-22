using HrHub.Abstraction.Data.Context;
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Abstraction.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace HrHub.Core.Data.Repository
{
    public partial class Repository<TEntity> where TEntity : class, IBaseEntity
    {
        public DbContextBase dbContext;
        protected DbSet<TEntity> DbSet { get; }

        public Repository(DbContextBase dbContext)
        {
            this.dbContext = dbContext;
            DbSet = this.dbContext.Set<TEntity>();
        }

        #region SyncMethods      
        public virtual void Add(TEntity entity) => DbSet.Add(entity);

        public virtual TEntity AddAndReturn(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public virtual void AddList(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);

        public virtual async Task AddOrUpdate(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entityEntry = dbContext.Entry(entity);
            if (entityEntry.State == EntityState.Detached)
            {
                // Not: DbSet.Contains memory yükü oluşturabilir, ID kontrolü daha performanslı olabilir.
                // Mevcut mantığı korudum.
                if (DbSet.Contains(entity))
                {
                    DbSet.Update(entity);
                }
                else
                {
                    await DbSet.AddAsync(entity, cancellationToken);
                }
            }
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null) => predicate == null ? DbSet.Count() : DbSet.Count(predicate);

        public virtual void Delete(TEntity entity) => DbSet.Remove(entity);

        public virtual void DeleteList(List<TEntity> entities) => DbSet.RemoveRange(entities);

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return dbContext.Set<TEntity>().Any(predicate);
        }

        public TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return DbSet.Max(selector);
        }

        public virtual TEntity UpdateAndReturn(TEntity entity)
        {
            DbSet.Update(entity);
            return entity;
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual void UpdateList(List<TEntity> entities) => DbSet.UpdateRange(entities);
        #endregion

        #region AsyncMethods

        public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
            => DbSet.AddAsync(entity, cancellationToken).AsTask();

        public virtual Task AddListAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            => DbSet.AddRangeAsync(entities, cancellationToken);

        public virtual async Task<TEntity> AddAndReturnAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default)
            => predicate == null ? DbSet.CountAsync(cancellationToken) : DbSet.CountAsync(predicate, cancellationToken);

        // DÜZELTME: Task.Run kaldırıldı (Anti-Pattern fix)
        public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);
            return Task.CompletedTask;
        }

        // DÜZELTME: Task.Run kaldırıldı (Anti-Pattern fix)
        public virtual Task DeleteListAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            DbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await dbContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        }

        public async Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                     Expression<Func<TEntity, bool>> predicate = null,
                                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                     CancellationToken cancellationToken = default)
        {
            var query = CreateQuery(predicate: predicate, include: include, whereIf: whereIf);
            return await query.MaxAsync(selector, cancellationToken);
        }

        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task UpdateListAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            DbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }
        #endregion

        #region SyncGetMethods

        public TEntity Get(Expression<Func<TEntity, bool>> predicate,
                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                           Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                           Expression<Func<TEntity, TEntity>> selector = null)
        {
            var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf);
            if (selector is null)
                return query.FirstOrDefault();

            return query.Select(selector).FirstOrDefault();
        }

        public TEntity GetWithNoLock(Expression<Func<TEntity, bool>> predicate,
                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                     Expression<Func<TEntity, TEntity>> selector = null)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf, noLock: true);
                if (selector is null)
                    return query.FirstOrDefault();

                return query.Select(selector).FirstOrDefault();
            }
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                            bool disableTracking = false)
        {
            var query = CreateQuery(predicate, orderBy, include, whereIf, disableTracking);
            return query.ToList();
        }

        public IEnumerable<TEntity> GetListWithNoLock(Expression<Func<TEntity, bool>> predicate = null,
                                                      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                      Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate, orderBy, include, whereIf);
                return query.ToList();
            }
        }

        public IEnumerable<TResult> GetListWithNoLock<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                               Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                               Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                               Expression<Func<TEntity, TResult>> selector = null)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate, orderBy, include, whereIf);

                // DÜZELTME: Selector Null Check
                if (selector is null)
                {
                    var list = query.ToList();
                    return list.Select(x => (TResult)(object)x).ToList();
                }

                return query.Select(selector).ToList();
            }
        }

        public IEnumerable<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
                                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                 Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                 Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                 int skip = 0,
                                                 int take = 20,
                                                 bool disableTracking = false)
        {
            var query = CreateQuery(predicate, orderBy, include, whereIf, disableTracking);
            return query.Skip(skip).Take(take).ToList();
        }

        public IEnumerable<TResult> GetPagedList<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                          Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                          Expression<Func<TEntity, TResult>> selector = null,
                                                          int skip = 0,
                                                          int take = 20,
                                                          bool disableTracking = false)
        {
            var query = CreateQuery(predicate, orderBy, include, whereIf, disableTracking: disableTracking);

            // DÜZELTME: Selector Null Check
            if (selector == null)
            {
                var list = query.Skip(skip).Take(take).ToList();
                return list.Select(x => (TResult)(object)x).ToList();
            }

            return query.Skip(skip).Take(take).Select(selector).ToList();
        }

        public IEnumerable<TEntity> GetPagedListWithNoLock(Expression<Func<TEntity, bool>> predicate = null,
                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                           Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                           int skip = 0,
                                                           int take = 20,
                                                           bool disableTracking = false)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate, orderBy, include, whereIf, disableTracking);
                return query.Skip(skip).Take(take).ToList();
            }
        }

        public IEnumerable<TResult> GetPagedListWithNoLock<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                                    Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                                    Expression<Func<TEntity, TResult>> selector = null,
                                                                    int skip = 0,
                                                                    int take = 20)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate, orderBy, include, whereIf);

                // DÜZELTME: Selector Null Check
                if (selector == null)
                {
                    var list = query.Skip(skip).Take(take).ToList();
                    return list.Select(x => (TResult)(object)x).ToList();
                }

                return query.Skip(skip).Take(take).Select(selector).ToList();
            }
        }

        #endregion

        #region AsyncGetMethods

        // DÜZELTME: Metot async hale getirildi ve null selector için cast işlemi eklendi
        public async Task<TResult> GetAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Expression<Func<TEntity, TResult>> selector = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
            CancellationToken cancellationToken = default)
        {
            var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf);

            if (selector == null)
            {
                var entity = await query.FirstOrDefaultAsync(cancellationToken);
                return (TResult)(object)entity;
            }

            return await query.Select(selector).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                            Expression<Func<TEntity, TEntity>> selector = null)
        {
            var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf);
            if (selector == null)
                return await query.FirstOrDefaultAsync();

            return await query.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                             Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                             Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                             CancellationToken cancellationToken = default)
        {
            var query = CreateQuery(predicate, orderBy, include, whereIf);
            return await query.ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public Task<TEntity> GetWithNoLockAsync(Expression<Func<TEntity, bool>> predicate,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                bool disableTracking = false,
                                                CancellationToken cancellationToken = default)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf, disableTracking: disableTracking);
                return query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                                      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                                      Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                                      Expression<Func<TEntity, TResult>> selector = null,
                                                                      CancellationToken cancellationToken = default)
        {
            var query = CreateQuery(predicate, orderBy, include, whereIf);

            // DÜZELTME: Null check
            if (selector == null)
            {
                var list = await query.ToListAsync(cancellationToken).ConfigureAwait(false);
                return list.Select(x => (TResult)(object)x).ToList();
            }

            return await query.Select(selector).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TResult>> GetListWithNoLockAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                                                Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                                                Expression<Func<TEntity, TResult>> selector = null,
                                                                                CancellationToken cancellationToken = default)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate, orderBy, include, whereIf);

                // DÜZELTME: Null check
                if (selector == null)
                {
                    var list = await query.ToListAsync(cancellationToken).ConfigureAwait(false);
                    return list.Select(x => (TResult)(object)x).ToList();
                }

                return await query.Select(selector).ToListAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                                  Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                                  int skip = 0,
                                                                  int take = 20,
                                                                  CancellationToken cancellationToken = default)
        {
            var query = CreateQuery(predicate, orderBy, include, whereIf);
            return await query.Skip(skip).Take(take).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> GetPagedListWithNoLockAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                                            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                                            int skip = 0,
                                                                            int take = 20,
                                                                            CancellationToken cancellationToken = default)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate, orderBy, include, whereIf);
                return await query.Skip(skip).Take(take).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                                           Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                                           Expression<Func<TEntity, TResult>> selector = null,
                                                                           int skip = 0,
                                                                           int take = 20,
                                                                           CancellationToken cancellationToken = default)
        {
            var query = CreateQuery(predicate, orderBy, include, whereIf);

            // DÜZELTME: Null check
            if (selector == null)
            {
                var list = await query.Skip(skip).Take(take).ToListAsync(cancellationToken).ConfigureAwait(false);
                return list.Select(x => (TResult)(object)x).ToList();
            }

            return await query.Skip(skip).Take(take).Select(selector).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TResult>> GetPagedListWithNoLockAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                                                     Expression<Func<TEntity, TResult>> selector = null,
                                                                                     int skip = 0,
                                                                                     int take = 20,
                                                                                     CancellationToken cancellationToken = default)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate, orderBy, include, whereIf);

                // DÜZELTME: Null check
                if (selector == null)
                {
                    var list = await query.Skip(skip).Take(take).ToListAsync(cancellationToken).ConfigureAwait(false);
                    return list.Select(x => (TResult)(object)x).ToList();
                }

                return await query.Skip(skip).Take(take).Select(selector).ToListAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        #endregion

        protected TransactionScope CreateTransaction(IsolationLevel isolationLevel)
        {
            return new TransactionScope
            (
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = isolationLevel,
                    Timeout = TransactionManager.MaximumTimeout // Opsiyonel, timeout hatası almamak için
                },
                TransactionScopeAsyncFlowOption.Enabled // Async işlemler için kritik
            );
        }

        protected IQueryable<TEntity> CreateQuery(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true,
            bool noLock = false)
        {
            IQueryable<TEntity> query = DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (noLock)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        protected IQueryable<TEntity> CreateQuery(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
            bool disableTracking = true,
            bool noLock = false)
        {
            IQueryable<TEntity> query = DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (noLock)
            {
                query = query.AsNoTrackingWithIdentityResolution();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (whereIf != null)
            {
                query = whereIf(query);
            }

            return query;
        }

        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                            bool disableTracking = true, bool noLock = false)
        {
            var query = CreateQuery(predicate, orderBy, include, whereIf, disableTracking, noLock);
            return query;
        }
    }
}

#region OldCode
//using HrHub.Abstraction.Data.Context;
//using HrHub.Abstraction.Data.EfCore.Repository;
//using HrHub.Abstraction.Domain;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Query;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using System.Transactions;

//namespace HrHub.Core.Data.Repository
//{
//    public partial class Repository<TEntity> where TEntity : class, IBaseEntity
//    {
//        public DbContextBase dbContext;
//        protected DbSet<TEntity> DbSet { get; }
//        public Repository(DbContextBase dbContext)
//        {
//            this.dbContext = dbContext;
//            DbSet = this.dbContext.Set<TEntity>();
//        }

//        #region SyncMethods     
//        public virtual void Add(TEntity entity) => DbSet.Add(entity);
//        public virtual TEntity AddAndReturn(TEntity entity)
//        {
//            DbSet.Add(entity);
//            return entity;
//        }
//        public virtual void AddList(IEnumerable<TEntity> entities) => DbSet.AddRange(entities);
//        public virtual async Task AddOrUpdate(TEntity entity, CancellationToken cancellationToken = default)
//        {
//            var entityEntry = dbContext.Entry(entity);
//            if (entityEntry.State == EntityState.Detached)
//            {
//                if (DbSet.Contains(entity))
//                {
//                    DbSet.Update(entity);
//                }
//                else
//                {
//                    await DbSet.AddAsync(entity);
//                }
//            }
//        }
//        public int Count(Expression<Func<TEntity, bool>> predicate = null) => DbSet.Count(predicate);
//        public virtual void Delete(TEntity entity) => DbSet.Remove(entity);
//        public virtual void DeleteList(List<TEntity> entities) => DbSet.RemoveRange(entities);
//        public bool Exists(Expression<Func<TEntity, bool>> predicate)
//        {
//            var result = dbContext.Set<TEntity>().Any(predicate);
//            return result;
//        }
//        public TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector)
//        {
//            return DbSet.Max(selector);
//        }
//        public virtual TEntity UpdateAndReturn(TEntity entity)
//        {
//            DbSet.Update(entity);
//            return entity;
//        }
//        public virtual void Update(TEntity entity)
//        {
//            DbSet.Update(entity);
//        }
//        public virtual void UpdateList(List<TEntity> entities) => DbSet.UpdateRange(entities);
//        #endregion

//        #region AsyncMethods
//        public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) => DbSet.AddAsync(entity, cancellationToken).AsTask();
//        public virtual Task AddListAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) => DbSet.AddRangeAsync(entities, cancellationToken);
//        public virtual async Task<TEntity> AddAndReturnAsync(TEntity entity, CancellationToken cancellationToken = default)
//        {
//            await DbSet.AddAsync(entity, cancellationToken);
//            return entity;
//        }
//        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default) => DbSet.CountAsync(predicate, cancellationToken);
//        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
//        {
//            await Task.Run(() =>
//            {
//                DbSet.Remove(entity);
//            }, cancellationToken);
//        }
//        public virtual async Task DeleteListAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
//        {
//            await Task.Run(() =>
//            {
//                DbSet.RemoveRange(entities);
//            }, cancellationToken);
//        }
//        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
//        {
//            var result = await dbContext.Set<TEntity>().AnyAsync(predicate);
//            return result;
//        }
//        public async Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
//                                                     Expression<Func<TEntity, bool>> predicate = null,
//                                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                     CancellationToken cancellationToken = default)
//        {
//            var query = CreateQuery(predicate: predicate, include: include, whereIf: whereIf);
//            return await query.MaxAsync(selector, cancellationToken);
//        }

//        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
//        {
//            DbSet.Update(entity);
//            return Task.CompletedTask;
//        }
//        public virtual Task UpdateListAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
//        {
//            UpdateListAsync(entities, cancellationToken);
//            return Task.CompletedTask;
//        }
//        #endregion




//        #region SyncGetMethods

//        public TEntity Get(Expression<Func<TEntity, bool>> predicate,
//                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                           Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                           Expression<Func<TEntity, TEntity>> selector = null)
//        {
//            var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf);
//            if (selector is null)
//                return query.FirstOrDefault();

//            return query.Select(selector).FirstOrDefault();
//        }

//        public TEntity GetWithNoLock(Expression<Func<TEntity, bool>> predicate,
//                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                     Expression<Func<TEntity, TEntity>> selector = null)
//        {
//            using (CreateTransaction(IsolationLevel.ReadUncommitted))
//            {
//                var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf, noLock: true);
//                if (selector is null)
//                    return query.FirstOrDefault();

//                return query.Select(selector).FirstOrDefault();
//            }
//        }

//        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null,
//                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                            bool disableTracking = false)
//        {
//            var query = CreateQuery(predicate, orderBy, include, whereIf, disableTracking);
//            return query.ToList();
//        }

//        public IEnumerable<TEntity> GetListWithNoLock(Expression<Func<TEntity, bool>> predicate = null,
//                                                      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                      Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null)
//        {
//            using (CreateTransaction(IsolationLevel.ReadUncommitted))
//            {
//                var query = CreateQuery(predicate, orderBy, include, whereIf);
//                return query.ToList();
//            }
//        }

//        public IEnumerable<TResult> GetListWithNoLock<TResult>(Expression<Func<TEntity, bool>> predicate = null,
//                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                     Expression<Func<TEntity, TResult>> selector = null)
//        {
//            using (CreateTransaction(IsolationLevel.ReadUncommitted))
//            {
//                var query = CreateQuery(predicate, orderBy, include, whereIf);
//                if (selector is null)
//                    return (IEnumerable<TResult>)query.ToList();
//                return query.Select(selector).ToList();
//            }
//        }

//        public IEnumerable<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
//                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                         Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                         int skip = 0,
//                                         int take = 20,
//                                         bool disableTracking = false)
//        {
//            var query = CreateQuery(predicate, orderBy, include, whereIf, disableTracking);
//            return query.Skip(skip).Take(take).ToList();
//        }

//        public IEnumerable<TResult> GetPagedList<TResult>(Expression<Func<TEntity, bool>> predicate = null,
//                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                  Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                  Expression<Func<TEntity, TResult>> selector = null,
//                                                  int skip = 0,
//                                                  int take = 20,
//                                                  bool disableTracking = false)
//        {
//            var query = CreateQuery(predicate, orderBy, include, whereIf, disableTracking: disableTracking);
//            return query.Skip(skip).Take(take).Select(selector).ToList();
//        }

//        public IEnumerable<TEntity> GetPagedListWithNoLock(Expression<Func<TEntity, bool>> predicate = null,
//                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                         Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                         int skip = 0,
//                                         int take = 20,
//                                         bool disableTracking = false)
//        {
//            using (CreateTransaction(IsolationLevel.ReadUncommitted))
//            {
//                var query = CreateQuery(predicate, orderBy, include, whereIf, disableTracking);
//                return query.Skip(skip).Take(take).ToList();
//            }
//        }

//        public IEnumerable<TResult> GetPagedListWithNoLock<TResult>(Expression<Func<TEntity, bool>> predicate = null,
//                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                  Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                  Expression<Func<TEntity, TResult>> selector = null,
//                                                  int skip = 0,
//                                                  int take = 20)
//        {
//            using (CreateTransaction(IsolationLevel.ReadUncommitted))
//            {
//                var query = CreateQuery(predicate, orderBy, include, whereIf);
//                return query.Skip(skip).Take(take).Select(selector).ToList();
//            }
//        }

//        #endregion

//        #region AsyncGetMethods

//        public async Task<TResult> GetAsync<TResult>(
//            Expression<Func<TEntity, bool>> predicate,
//            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//            Expression<Func<TEntity, TResult>> selector = null,
//            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//            CancellationToken cancellationToken = default)
//        {
//            var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf);

//            // KONTROL: Eğer selector null ise
//            if (selector == null)
//            {
//                // Entity'yi olduğu gibi çekiyoruz
//                var entity = await query.FirstOrDefaultAsync(cancellationToken);

//                // Entity'yi TResult tipine zorla dönüştürüyoruz (boxing/unboxing)
//                // Not: Bu durumda TResult'ın TEntity ile aynı tip olması beklenir.
//                return (TResult)(object)entity;
//            }

//            // Selector doluysa projeksiyon yaparak çekiyoruz
//            return await query.Select(selector).FirstOrDefaultAsync(cancellationToken);
//        }

//        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
//                   Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                   Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                   Expression<Func<TEntity, TEntity>> selector = null)
//        {
//            var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf);
//            if(selector == null)
//                return await query.FirstOrDefaultAsync();

//            return await query.Select(selector).FirstOrDefaultAsync();
//        }

//        public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null,
//                                                             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                             Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                             Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                             CancellationToken cancellationToken = default)
//        {
//            var query = CreateQuery(predicate, orderBy, include, whereIf);
//            return await query.ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
//        }

//        public Task<TEntity> GetWithNoLockAsync(Expression<Func<TEntity, bool>> predicate,
//                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                bool disableTracking = false,
//                                                CancellationToken cancellationToken = default)
//        {
//            using (CreateTransaction(IsolationLevel.ReadUncommitted))
//            {
//                var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf, disableTracking: disableTracking);
//                return query.FirstOrDefaultAsync(cancellationToken: cancellationToken);
//            }
//        }

//        public async Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
//                                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                              Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                              Expression<Func<TEntity, TResult>> selector = null,
//                                                              CancellationToken cancellationToken = default)
//        {
//            var query = CreateQuery(predicate, orderBy, include, whereIf);
//            return await query.Select(selector).ToListAsync().ConfigureAwait(false);
//        }

//        public async Task<IEnumerable<TResult>> GetListWithNoLockAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
//                                                              Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                              Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                              Expression<Func<TEntity, TResult>> selector = null,
//                                                              CancellationToken cancellationToken = default)
//        {
//            using (CreateTransaction(IsolationLevel.ReadUncommitted))
//            {
//                var query = CreateQuery(predicate, orderBy, include, whereIf);
//                return await query.Select(selector).ToListAsync().ConfigureAwait(false);
//            }
//        }

//        public async Task<IEnumerable<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null,
//                                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                          Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                          int skip = 0,
//                                                          int take = 20,
//                                                          CancellationToken cancellationToken = default)
//        {
//            var query = CreateQuery(predicate, orderBy, include, whereIf);
//            return await query.Skip(skip).Take(take).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
//        }

//        public async Task<IEnumerable<TEntity>> GetPagedListWithNoLockAsync(Expression<Func<TEntity, bool>> predicate = null,
//                                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                          Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                          int skip = 0,
//                                                          int take = 20,
//                                                          CancellationToken cancellationToken = default)
//        {
//            using (CreateTransaction(IsolationLevel.ReadUncommitted))
//            {
//                var query = CreateQuery(predicate, orderBy, include, whereIf);
//                return await query.Skip(skip).Take(take).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
//            }
//        }

//        public async Task<IEnumerable<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
//                                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                                           Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                                           Expression<Func<TEntity, TResult>> selector = null,
//                                                                           int skip = 0,
//                                                                           int take = 20,
//                                                                           CancellationToken cancellationToken = default)
//        {
//            var query = CreateQuery(predicate, orderBy, include, whereIf);
//            return await query.Skip(skip).Take(take).Select(selector).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
//        }


//        public async Task<IEnumerable<TResult>> GetPagedListWithNoLockAsync<TResult>(Expression<Func<TEntity, bool>> predicate = null,
//                                                                   Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                                                   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                                                   Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                                                   Expression<Func<TEntity, TResult>> selector = null,
//                                                                   int skip = 0,
//                                                                   int take = 20,
//                                                                   CancellationToken cancellationToken = default)
//        {
//            using (CreateTransaction(IsolationLevel.ReadUncommitted))
//            {
//                var query = CreateQuery(predicate, orderBy, include, whereIf);
//                return await query.Skip(skip).Take(take).Select(selector).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
//            }
//        }

//        #endregion


//        protected TransactionScope CreateTransaction(IsolationLevel isolationLevel)
//        {
//            return new TransactionScope
//            (
//                TransactionScopeOption.Required,
//                new TransactionOptions
//                {
//                    IsolationLevel = isolationLevel,
//                }
//            );
//        }

//        protected IQueryable<TEntity> CreateQuery(Expression<Func<TEntity, bool>> predicate = null,
//            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//            bool disableTracking = true,
//            bool noLock = false)
//        {
//            IQueryable<TEntity> query = DbSet;
//            if (disableTracking)
//            {
//                query = query.AsNoTracking();
//            }

//            if (noLock)
//            {
//                query = query.AsNoTrackingWithIdentityResolution();
//            }

//            if (include != null)
//            {
//                query = include(query);
//            }

//            if (predicate != null)
//            {
//                query = query.Where(predicate);
//            }
//            if (orderBy != null)
//            {
//                query = orderBy(query);
//            }

//            return query;
//        }

//        protected IQueryable<TEntity> CreateQuery(Expression<Func<TEntity, bool>> predicate = null,
//            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//            bool disableTracking = true,
//            bool noLock = false)
//        {
//            IQueryable<TEntity> query = DbSet;
//            if (disableTracking)
//            {
//                query = query.AsNoTracking();
//            }

//            if (noLock)
//            {
//                query = query.AsNoTrackingWithIdentityResolution();
//            }

//            if (include != null)
//            {
//                query = include(query);
//            }

//            if (predicate != null)
//            {
//                query = query.Where(predicate);
//            }
//            if (orderBy != null)
//            {
//                query = orderBy(query);
//            }
//            if (whereIf != null)
//            {
//                query = whereIf(query);
//            }

//            return query;
//        }
//        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate = null,
//                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
//                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//                                            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
//                                            bool disableTracking = true, bool noLock = false)
//        {
//            IQueryable<TEntity> query = DbSet;
//            if (disableTracking)
//            {
//                query = query.AsNoTracking();
//            }

//            if (noLock)
//            {
//                query = query.AsNoTrackingWithIdentityResolution();
//            }

//            if (include != null)
//            {
//                query = include(query);
//            }

//            if (predicate != null)
//            {
//                query = query.Where(predicate);
//            }
//            if (orderBy != null)
//            {
//                query = orderBy(query);
//            }
//            if (whereIf != null)
//            {
//                query = whereIf(query);
//            }

//            return query;
//        }
//    }
//} 
#endregion
