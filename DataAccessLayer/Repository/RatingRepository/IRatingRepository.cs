using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;

namespace DataAccessLayer.Repository.RatingRepository
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<float> GetAverageAsync(int id);
        Task<Rating> GetByIdAsync(int recipecId, int userId);
    }
}
