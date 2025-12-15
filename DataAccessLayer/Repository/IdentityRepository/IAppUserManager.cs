using DomainLayer.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.IdentityRepository
{
    public interface IAppUserManager
    {
        Task<int> CountAsync();
        Task<List<UserApp>> GetAllAsNoTrackingAsync(int skip, int pageSize, string orderBy, string orderDirection, string role, bool? isActive);
        Task<IdentityResult> CreateAsync(UserApp user, string password);
        Task<IdentityResult> UpdateAsync(UserApp user);
        Task<IdentityResult> DeleteAsync(UserApp user);
        Task<IdentityResult> AddToRoleAsync(UserApp user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(UserApp user, string role);
        Task<IList<string>> GetRolesAsync(UserApp user);
        Task<UserApp> FindByIdAsync(string id);
        Task<UserApp> FindByNameAsync(string userName);
        Task<UserApp> FindByEmailAsync(string email);
        Task<bool> IsInRoleAsync(UserApp user, string role);
        Task<UserApp> GetByIdAsync(int id);

        Task<IdentityResult> ChangePasswordAsync(UserApp user, string currentPassword, string newPassword);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
