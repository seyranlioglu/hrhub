using HrHub.Abstraction.Data.Context;
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Abstraction.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HrHub.Core.Data.Repository
{
    public partial class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        public DbContextBase dbContext;
        protected DbSet<TEntity> DbSet { get; }
        public EntityRepository(DbContextBase dbContext)
        {
            this.dbContext = dbContext;
            DbSet = this.dbContext.Set<TEntity>();
        }
        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Add(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return DbSet.AddAsync(entity, cancellationToken).AsTask();
        }

        public virtual Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return DbSet.AddRangeAsync(entities, cancellationToken);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return DbSet.Count(predicate);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            return DbSet.CountAsync(predicate, cancellationToken);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void Delete(List<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                DbSet.Remove(entity);
            }, cancellationToken);
        }

        public virtual async Task DeleteAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                DbSet.RemoveRange(entities);
            },cancellationToken);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual async Task AddOrUpdate(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entityEntry = dbContext.Entry(entity);
            if (entityEntry.State == EntityState.Detached)
            {
                if (DbSet.Contains(entity))
                {
                    DbSet.Update(entity);
                }
                else
                {
                    await DbSet.AddAsync(entity);
                }
            }
        }

        public virtual void Update(List<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }

        public virtual Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            UpdateAsync(entities, cancellationToken);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var result = await ExistsAsync(predicate, cancellationToken);
            return result;
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return Exists(predicate);
        }

        public TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return DbSet.Max(selector);
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

        public TEntity Get(Expression<Func<TEntity, bool>> predicate,
                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                           Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null)
        {
            var query = CreateQuery(predicate: predicate, include: include, whereIf: whereIf);
            return query.SingleOrDefault();
        }

        public TEntity GetWithNoLock(Expression<Func<TEntity, bool>> predicate,
                   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                   Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate: predicate, include: include, whereIf: whereIf);
                return query.SingleOrDefault();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                   Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                   Expression<Func<TEntity, TEntity>> selector = null)
        {
            var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf);
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
                return query.Select(selector).FirstOrDefault();
            }
        }

        public TResult Get<TResult>(Expression<Func<TEntity, bool>> predicate,
                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                    Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                    Expression<Func<TEntity, TResult>> selector = null)
        {
            var query = CreateQuery(predicate: predicate, include: include, whereIf: whereIf);
            return query.Select(selector).SingleOrDefault();
        }

        public TResult GetWithNoLock<TResult>(Expression<Func<TEntity, bool>> predicate,
                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                            Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                            Expression<Func<TEntity, TResult>> selector = null)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate: predicate, include: include, whereIf: whereIf);
                return query.Select(selector).SingleOrDefault();
            }
        }

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
                                      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                      Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                      bool disableTracking = false,
                                      CancellationToken cancellationToken = default)
        {
            var query = CreateQuery(predicate: predicate, include: include, whereIf: whereIf, disableTracking: disableTracking);
            return query.SingleOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public Task<TEntity> GetWithNoLockAsync(Expression<Func<TEntity, bool>> predicate,
                              Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                              Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                              bool disableTracking = false,
                              CancellationToken cancellationToken = default)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate: predicate, include: include, whereIf: whereIf, disableTracking: disableTracking);
                return query.SingleOrDefaultAsync(cancellationToken: cancellationToken);
            }
        }

        public Task<TResult> GetAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
                                               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                               Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                               Expression<Func<TEntity, TResult>> selector = null,
                                               Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                               CancellationToken cancellationToken = default)
        {
            var query = CreateQuery(predicate: predicate, orderBy: orderBy, include: include, whereIf: whereIf);
            return query.Select(selector).SingleOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public Task<TResult> GetWithNoLockAsync<TResult>(Expression<Func<TEntity, bool>> predicate,
                                       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                       Expression<Func<TEntity, TResult>> selector = null,
                                       Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                       CancellationToken cancellationToken = default)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate: predicate, include: include, whereIf: whereIf);
                return query.Select(selector).SingleOrDefaultAsync(cancellationToken: cancellationToken);
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

        public IEnumerable<TResult> GetList<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                     Expression<Func<TEntity, TResult>> selector = null)
        {
            var query = CreateQuery(predicate, orderBy, include, whereIf);
            return query.Select(selector).ToList();
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
                return query.Select(selector).ToList();
            }
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

        public async Task<IEnumerable<TEntity>> GetListWithNoLockAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                     CancellationToken cancellationToken = default)
        {
            using (CreateTransaction(IsolationLevel.ReadUncommitted))
            {
                var query = CreateQuery(predicate, orderBy, include, whereIf);
                return await query.ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
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
            return await query.Select(selector).ToListAsync().ConfigureAwait(false);
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
                return await query.Select(selector).ToListAsync().ConfigureAwait(false);
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

        public IEnumerable<TResult> GetPagedList<TResult>(Expression<Func<TEntity, bool>> predicate = null,
                                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                          Func<IQueryable<TEntity>, IQueryable<TEntity>> whereIf = null,
                                                          Expression<Func<TEntity, TResult>> selector = null,
                                                          int skip = 0,
                                                          int take = 20)
        {
            var query = CreateQuery(predicate, orderBy, include, whereIf);
            return query.Skip(skip).Take(take).Select(selector).ToList();
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
                return query.Skip(skip).Take(take).Select(selector).ToList();
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
            return await query.Skip(skip).Take(take).Select(selector).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
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
                return await query.Skip(skip).Take(take).Select(selector).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            }
        }

        protected TransactionScope CreateTransaction(IsolationLevel isolationLevel)
        {
            return new TransactionScope
            (
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = isolationLevel,
                }
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
    }
}
