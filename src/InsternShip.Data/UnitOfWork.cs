using InsternShip.Data.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace InsternShip.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly RecruitmentDB _dbContext;
        private IDbContextTransaction? _transaction = null;

        public UnitOfWork(RecruitmentDB dbContext)
        {
            _dbContext = dbContext;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public void BeginTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }
        }
    }

}
