using Microsoft.EntityFrameworkCore.Storage;
using WalletKata.Repositories.Interfaces;

namespace WalletKata.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KataDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(KataDbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}
