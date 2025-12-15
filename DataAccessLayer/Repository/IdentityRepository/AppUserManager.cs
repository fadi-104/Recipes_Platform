using DomainLayer.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Dynamic.Core;

namespace DataAccessLayer.Repository.IdentityRepository
{
    public class AppUserManager : UserManager<UserApp>, IAppUserManager
    {
        private readonly ApplicationDbContext _dbContext;
        public AppUserManager(IUserStore<UserApp> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<UserApp> passwordHasher,
            IEnumerable<IUserValidator<UserApp>> userValidators,
            IEnumerable<IPasswordValidator<UserApp>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<UserApp>> logger,
            ApplicationDbContext dbContext) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _dbContext = dbContext;
            
        }

        public async Task<List<UserApp>> GetAllAsNoTrackingAsync(int skip, int pageSize, string orderBy, string orderDirection, string role, bool? isActive)
        {
            if (orderDirection == "desc")
                orderBy = $"{orderBy} desc";
            if (isActive is null)
            {
                return await (from user in Users
                             join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                             join Role in _dbContext.Roles on userRole.RoleId equals Role.Id
                             where Role.Name == role
                             select user)
                             .AsNoTracking()
                             .OrderBy(orderBy)
                             .Skip(skip)
                             .Take(pageSize)
                             .ToListAsync();
            }
            else
            {
                return await (from user in Users
                             join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                             join Role in _dbContext.Roles on userRole.RoleId equals Role.Id
                             where Role.Name == role && user.IsActive == isActive
                             select user)
                             .AsNoTracking()
                             .OrderBy(orderBy)
                             .Skip(skip)
                             .Take(pageSize)
                             .ToListAsync();
            }

        }

        public async Task<UserApp> GetByIdAsync(int id)
        {
            return await Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<int> CountAsync()
        {
            return await Users.CountAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
    }
}
