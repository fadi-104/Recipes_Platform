using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;

namespace DataAccessLayer.Repository.MessageRepository
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<List<Message>> GetAllMessageAsync(int skip, int take, int senderId, int recevierId);
        Task<Message> GetByIdAsync(int id);
    }
}
