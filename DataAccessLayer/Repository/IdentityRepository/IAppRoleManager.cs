using DomainLayer.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.IdentityRepository
{
    public interface IAppRoleManager
    {
        Task<IdentityResult> CreateAsync(RoleApp role);
        Task<IdentityResult> DeleteAsync(RoleApp role);
        Task<IdentityResult> UpdateAsync(RoleApp role);
        Task<List<RoleApp>> GetAllAsync();
        Task<bool> RoleExistsAsync(string roleName);
    }
}
