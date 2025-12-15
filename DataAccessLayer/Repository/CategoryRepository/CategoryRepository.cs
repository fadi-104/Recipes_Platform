using DataAccessLayer.BaseRepository;
using DomainLayer.Entites;


namespace DataAccessLayer.Repository.CategoryRepository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {  
        }
    }
}
