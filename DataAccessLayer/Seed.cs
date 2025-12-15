using DataAccessLayer.Repository.IdentityRepository;
using DomainLayer.Entites;


namespace DataAccessLayer
{
    public class Seed
    {
        private readonly IAppRoleManager _roleManager;
        public Seed(IAppRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task UseSeedAsync()
        {
            await CreateRoleAsync();
        }

        private async Task CreateRoleAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var admin = new RoleApp()
                {
                    Name = "Admin",
                };
                await _roleManager.CreateAsync(admin);
            }

            if (!await _roleManager.RoleExistsAsync("Chef"))
            {
                var chef = new RoleApp()
                {
                    Name = "Chef",
                };
                await _roleManager.CreateAsync(chef);
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                var user = new RoleApp()
                {
                    Name = "User",
                };
                await _roleManager.CreateAsync(user);
            }

        }

    }
}
