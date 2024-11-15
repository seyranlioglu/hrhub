using HrHub.Abstraction.Domain;
using HrHub.Core.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.Data.UnitOfWork
{
    public interface IUnitOfWork<TDBContext> : IDisposable
    {
        IDbContextTransaction CurrentTransaction { get; set; }

        void SaveChanges();

        void BeginTransaction();

        void CommitTransaction();
        void RollBackTransaction();

        void JoinExistingTransaction(IUnitOfWork<TDBContext> unitOfWork);

        void BeginTransaction(IsolationLevel level);

        int SetIsolationLevel(IsolationLevel level);

        void OpenConnection();

        void CloseConnection();

        #region AsyncMethods
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        Task RollBackTransactionAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default);
        Task TryConnectDbAsync();
        Repository<TEntity> CreateRepository<TEntity>() where TEntity : class, IBaseEntity;

        #endregion
    }
}
