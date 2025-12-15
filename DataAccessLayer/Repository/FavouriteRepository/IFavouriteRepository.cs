using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;

namespace DataAccessLayer.Repository.FavouriteRepository
{
    public interface IFavouriteRepository : IRepository<Favourite>
    {
        Task<List<Recipec>> GetFavouriteAsync(int userId);
    }
}
