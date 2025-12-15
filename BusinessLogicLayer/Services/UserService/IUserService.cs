using Core.Model;
using DomainLayer.Requests;
using DomainLayer.Responses;


namespace BusinessLogicLayer.Services.UserService
{
    public interface IUserService
    {
        Task ChangePasswordAsync(ChangePasswordRequest request);
        Task CreateAsync(UserRequest request);
        Task DeleteAsync(int id);
        Task<PagedResponse<List<UserResponse>>> GetAllAsync(TableOptions options, string role, bool? isActive);
        Task<UserResponse> GetAsync(int id);
        Task<UserResponse> GetByUserNameAsync(string userName);
        Task<TokenResponse> Login(LoginRequeste request);
        Task UpdateAsync(UserRequest request);
    }
}
