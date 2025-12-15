using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace DataAccessLayer.Repository.ImageReository
{
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        private readonly DbSet<Image> _dbSet;
        private readonly ApplicationDbContext _dbContext;
        public ImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<Image>();
            _dbContext = dbContext;
        }

        public async Task<List<Image>> GetAllByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .Where(x => x.RecipecId == id)
                .ToListAsync();
        }

        public async Task AddRangeAsync(List<Image> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
    }
}
