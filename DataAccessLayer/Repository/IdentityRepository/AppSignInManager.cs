using Core.Model;
using DomainLayer.Entites;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace DataAccessLayer.Repository.IdentityRepository
{
    public class AppSignInManager : SignInManager<UserApp>, IAppSignInManager
    {
        private readonly IConfiguration _configuration;

        public AppSignInManager(UserManager<UserApp> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<UserApp> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<UserApp>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<UserApp> confirmation,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor,
                  logger, schemes, confirmation
                  )
        {
            _configuration = configuration;
        }

        public async Task<TokenResponse> GenerateUserTokens(UserApp user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var role = (await UserManager.GetRolesAsync(user)).FirstOrDefault();

            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier,user.Id.ToString()),
                new (ClaimTypes.Name,user.FirstName),
                new (ClaimTypes.Surname,user.LastName),
                new (ClaimTypes.Email,user.Email ?? ""),
                new (ClaimTypes.Role,role ?? ""),
                new ("Logo", user.Image ?? ""),
            };

            var jwtKey = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var key = Encoding.UTF8.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = issuer,
                Audience = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = Guid.NewGuid().ToString(),
            };
        }

    }
}
