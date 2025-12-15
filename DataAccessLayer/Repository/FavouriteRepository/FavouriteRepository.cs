using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer.Repository.FavouriteRepository
{
    public class FavouriteRepository : GenericRepository<Favourite>, IFavouriteRepository
    {
        private readonly DbSet<Favourite> _dbSet;
        public FavouriteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<Favourite>();
        }

        public async Task<List<Recipec>> GetFavouriteAsync(int userId)
        {
                var query = await QueryAllNoTracking().AsNoTracking()
                .Where(f => f.UserId == userId)
                .Include(x => x.Recipe)
                .Include(x => x.Recipe.Category)
                .Select(x =>  new Recipec
                    {
                        Id = x.Recipe.Id,
                        Name = x.Recipe.Name,
                        Description = x.Recipe.Description,
                        CategoryId = x.Recipe.CategoryId,
                        BaseImage = x.Recipe.BaseImage,
                        Category = new Category
                        {
                            Name = x.Recipe.Category.Name
                        }
                    }
                 )
                .ToListAsync();

            return query;
        }
    }
}
