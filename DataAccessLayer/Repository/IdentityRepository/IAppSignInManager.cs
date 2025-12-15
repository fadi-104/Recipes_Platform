using Core.Model;
using DomainLayer.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.IdentityRepository
{
    public interface IAppSignInManager
    {
        Task<SignInResult> CheckPasswordSignInAsync(UserApp user, string password, bool lockoutOnFailure);
        Task<TokenResponse> GenerateUserTokens(UserApp user);
    }
}
