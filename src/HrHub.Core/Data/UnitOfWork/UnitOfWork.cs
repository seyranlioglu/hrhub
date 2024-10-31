using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using HrHub.Abstraction.Data.EfCore.UnitOfwork;

namespace HrHub.Core.Data.UnitOfWork
{
    public abstract class UnitOfWork<TDBContext> : IUnitOfWork<TDBContext> where TDBContext : DbContext
    {
        private bool disposedValue = false; // To detect redundant calls
        public TDBContext currentDBContext { get; set; }

        public IDbContextTransaction CurrentTransaction { get; set; }

        public UnitOfWork(TDBContext context)
        {
            currentDBContext = context;
        }

        public void BeginTransaction()
        {
            CurrentTransaction = currentDBContext.Database.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel level)
        {
            CurrentTransaction = currentDBContext.Database.BeginTransaction(level);
        }

        public void CommitTransaction()
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Commit();
            }
        }

        public void RollBackTransaction()
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Rollback();
            }
        }

        public void SaveChanges()
        {
            currentDBContext.SaveChanges();
        }

        public void JoinExistingTransaction(IUnitOfWork<TDBContext> unitOfWork)
        {
            currentDBContext.Database.UseTransaction(unitOfWork.CurrentTransaction.GetDbTransaction());
        }

        public async Task TryConnectDbAsync()
        {
            await currentDBContext.Database.CanConnectAsync();
        }

        public int SetIsolationLevel(IsolationLevel level)
        {
            string text = "SET TRANSACTION ISOLATION LEVEL ";
            return RelationalDatabaseFacadeExtensions.ExecuteSqlRaw(sql: level switch
            {
                IsolationLevel.ReadCommitted => text + "READ COMMITTED",
                IsolationLevel.ReadUncommitted => text + "READ UNCOMMITTED",
                IsolationLevel.RepeatableRead => text + "REPEATABLE READ",
                IsolationLevel.Serializable => text + "SERIALIZABLE",
                IsolationLevel.Snapshot => text + "SNAPSHOT",
                _ => text + "READ COMMITTED",
            }, databaseFacade: currentDBContext.Database, parameters: Array.Empty<object>());
        }

        public void CloseConnection()
        {
            if (currentDBContext.Database.CanConnect())
            {
                currentDBContext.Database.CloseConnection();
            }
        }

        public void OpenConnection()
        {
            if (!currentDBContext.Database.CanConnect())
            {
                currentDBContext.Database.OpenConnection();
            }
        }

        #region AsyncMethods
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await currentDBContext.SaveChangesAsync(cancellationToken: cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            CurrentTransaction = await currentDBContext.Database.BeginTransactionAsync(cancellationToken: cancellationToken);
        }

        public async Task BeginTransactionAsync(IsolationLevel level, CancellationToken cancellationToken = default)
        {
            CurrentTransaction = await currentDBContext.Database.BeginTransactionAsync(level, cancellationToken: cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (CurrentTransaction != null)
            {
                await CurrentTransaction.CommitAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task RollBackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (CurrentTransaction != null)
            {
                await CurrentTransaction.RollbackAsync(cancellationToken: cancellationToken);
            }
        }

        #endregion

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UnitOfWork()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
