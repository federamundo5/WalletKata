using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace WalletKata.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(long id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetByCustomFilterAsync(Expression<Func<TEntity, bool>> filter);


    }

}
