using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WalletKata.Repositories.Interfaces;

namespace WalletKata.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly KataDbContext _context;

        public Repository(KataDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> GetByIdAsync(long id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetByCustomFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

  
    }
}
