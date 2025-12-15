using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace DataAccessLayer.Repository.MessageRepository
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly DbSet<Message> _dbSet;
        public MessageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<Message>();
        }

        public async Task<List<Message>> GetAllMessageAsync(int skip, int take, int senderId, int recevierId)
        {
            var orderBy = "id desc";

            return await QueryAllNoTracking()
                .Where(x => (x.SenderId == senderId && x.ReceiverId == recevierId) || (x.SenderId == recevierId && x.ReceiverId == senderId))
                .OrderBy(orderBy)
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

        }

        public async Task<Message> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .Include(x => x.Sender)
                .Include(x => x.Receiver)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
