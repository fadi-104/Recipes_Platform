using DomainLayer.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repository.IdentityRepository
{
    public class AppRoleManager : RoleManager<RoleApp>, IAppRoleManager
    {
        private readonly ApplicationDbContext _dbContext;
        public AppRoleManager(IRoleStore<RoleApp> store,
        IEnumerable<IRoleValidator<RoleApp>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RoleManager<RoleApp>> logger, ApplicationDbContext dbContext) : base (store, roleValidators, keyNormalizer, errors, logger)
        {
            _dbContext = dbContext;
        }

        public async Task<List<RoleApp>> GetAllAsync()
        {
            return await _dbContext.Roles.ToListAsync();
        }
    }
}
