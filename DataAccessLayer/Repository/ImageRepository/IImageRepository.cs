using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccessLayer.Repository.ImageReository
{
    public interface IImageRepository : IRepository<Image>
    {
        Task AddRangeAsync(List<Image> entities);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<List<Image>> GetAllByIdAsync(int id);
    }
}
