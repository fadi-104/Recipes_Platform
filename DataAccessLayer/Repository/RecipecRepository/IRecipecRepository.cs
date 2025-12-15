using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;

namespace DataAccessLayer.Repository.RecipecRepository
{
    public interface IRecipecRepository : IRepository<Recipec>
    {
        Task AddViewCountAsync(Recipec recipec);
        Task<List<Recipec>> GetAllAsNoTracking(int skip, int pageSize, string orderBy, string orderDirection, string? name, bool? isPublished, int? CategoryId, DateTime? date);
        Task<Recipec> GetByIdAsync(int id);
    }
}
