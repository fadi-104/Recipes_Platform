using Microsoft.EntityFrameworkCore.Storage;


namespace DataAccessLayer.BaseRepository
{
    public interface IRepository<TEntity>
    {
        Task<List<TEntity>> GetAllNoTrackingAsync();
        Task<TEntity> FindAsync(int id);
        Task<TEntity> FindNoTrackingAsync(int id);
        Task<int> CountAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
