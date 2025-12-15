using DomainLayer.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DataAccessLayer
{
    public class ApplicationDbContext : IdentityDbContext<UserApp, RoleApp, int>
    {
        DbSet<Recipec> Recipecs { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Image> Images { get; set; }
        DbSet<Rating> Ratings { get; set; }
        DbSet<Favourite> Favourites { get; set; }
        DbSet<Message> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }
    }
}
