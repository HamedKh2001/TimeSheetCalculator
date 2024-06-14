using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel.Contracts.Infrastructure;
using System.Threading.Tasks;

namespace SharedKernel.Services
{
    public class UnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly T _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(T context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            if (_transaction != null)
            {
                return;
            }

            _transaction = _context.Database.BeginTransaction();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Commit()
        {
            if (_transaction == null)
            {
                return;
            }

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public async Task SaveAndCommitAsync()
        {
            await SaveChangesAsync();
            Commit();
        }

        public void Rollback()
        {
            if (_transaction == null)
            {
                return;
            }

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public void ClearChangeTracker()
        {
            _context.ChangeTracker.Clear();
        }

        public void Dispose()
        {
            if (_transaction == null)
            {
                return;
            }

            _transaction.Dispose();
            _transaction = null;
        }
    }
}
