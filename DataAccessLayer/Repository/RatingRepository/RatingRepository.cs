using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.RatingRepository
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        private readonly DbSet<Rating> _dbSet;
        public RatingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<Rating>();
        }

        public async Task<Rating> GetByIdAsync(int recipecId, int userId)
        {
            var rating = await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(r => r.RecipecId == recipecId && r.UserId == userId);
            return rating;
        }

        public async Task<float> GetAverageAsync(int id)
        {
            var rate = await _dbSet.AsNoTracking()
                .Where(r => r.RecipecId == id)
                .ToListAsync();

            if (rate.Count == 0 || rate is null)
                return 0f;
           
            var average = rate.Average(x => x.Rate);

            return average;

        }

    }
}
