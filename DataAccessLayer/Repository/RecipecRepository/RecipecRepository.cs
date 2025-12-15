using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace DataAccessLayer.Repository.RecipecRepository
{
    public class RecipecRepository : GenericRepository<Recipec>, IRecipecRepository
    {
        private readonly DbSet<Recipec> _dbSet;
        private readonly ApplicationDbContext _dbContext;
        public RecipecRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<Recipec>();
            _dbContext = dbContext;
        }

        public async Task<List<Recipec>> GetAllAsNoTracking(int skip, int pageSize, string orderBy, string orderDirection, string? name, bool? isPublished, int? CategoryId, DateTime? date)
        {
            if(orderDirection == "desc")
                orderBy = $"{orderBy} desc";
            var query = QueryAllNoTracking();
            
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(r => r.Name.Contains(name));
            }
            if (isPublished.HasValue)
            {
                query = query.Where(r => r.IsPublished == isPublished.Value);
            }
            if (CategoryId > 0)
            {
                query = query.Where(r => r.CategoryId == CategoryId);
            }
            if (date.HasValue)
            {
                query = query.Where(d => d.CreatedDate.Date == date.Value);
            }

            query = query.OrderBy(orderBy)
                .Include(x => x.Category)
                .Include(x => x.Chef)
                .Skip(skip)
                .Take(pageSize);

            return await query.Select(x => new Recipec()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                BaseImage = x.BaseImage,
                CategoryId = x.CategoryId,
                CookTime = x.CookTime,
            }).ToListAsync();

        }

        public async Task<Recipec> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Chef)
                .FirstOrDefaultAsync(x => x.Id == id);

        }


        public async Task AddViewCountAsync(Recipec recipec)
        {
            recipec.ViewCount += 1;
            _dbSet.Update(recipec);
            await _dbContext.SaveChangesAsync();
        }

    }
}
