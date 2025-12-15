using AutoMapper;
using BusinessLogicLayer.Services.Storage;
using Core.Exceptions;
using Core.Model;
using DataAccessLayer.Repository.IdentityRepository;
using DomainLayer.Entites;
using DomainLayer.Requests;
using DomainLayer.Responses;
using System.Text.RegularExpressions;



namespace BusinessLogicLayer.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IAppUserManager _appUserManager;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IAppSignInManager _appSignInManager;
        public UserService(IAppUserManager appUserManager, IStorageService storageService, IMapper mapper, IAppSignInManager appSignInManager)
        {
            _appUserManager = appUserManager;
            _storageService = storageService;
            _mapper = mapper;
            _appSignInManager = appSignInManager;
        }

        public async Task<PagedResponse<List<UserResponse>>> GetAllAsync(TableOptions options, string role, bool? isActive)
        {
            if (role is null)
            {
                throw new DataValidationException("Role must be set");
            }

            var totalCount = await _appUserManager.CountAsync();
            
            var list = await _appUserManager.GetAllAsNoTrackingAsync(options.Skip, options.PageSize, options.OrderBy, options.OrderByDirection, role, isActive);
            var response = list.Select(x => _mapper.Map<UserResponse>(x)).ToList();

            return new PagedResponse<List<UserResponse>>
            {
                Data = response,
                TotalCount = totalCount,
            };
        }

        public async Task<UserResponse> GetAsync(int id)
        {
            var entity = await _appUserManager.GetByIdAsync(id);
            if (entity is null)
                throw new DataNotFoundException("User not found");

            var response = _mapper.Map<UserResponse>(entity);
            return response;
        }

        public async Task<UserResponse> GetByUserNameAsync(string userName)
        {
            var entity = await _appUserManager.FindByNameAsync(userName);
            if (entity is null)
                throw new DataNotFoundException("User not found");

            var response = _mapper.Map<UserResponse>(entity);
            return response;
        }

        public async Task CreateAsync(UserRequest request)
        {
            using (var transaction = await _appUserManager.BeginTransactionAsync())
            {
                if (request.Id > 0)
                    throw new DataValidationException("Id musn't to be set");
                var entity = _mapper.Map<UserApp>(request);
                entity.Image = await _storageService.FileSaveAsync(request.Image, "/Project_Structure/Project_Structure/wwwroot/Image/User");

                var result = await _appUserManager.CreateAsync(entity, request.Password);
                if (!result.Succeeded)
                    throw new DataValidationException(result.Errors.First().Description);

                result = await _appUserManager.AddToRoleAsync(entity, request.Role);
                if (!result.Succeeded)
                    throw new DataValidationException(result.Errors.First().Description);

                await transaction.CommitAsync();
            }
        }

        public async Task UpdateAsync(UserRequest request)
        {
            using (var transaction = await _appUserManager.BeginTransactionAsync())
            {
                if(!request.Id.HasValue)
                    throw new DataValidationException("Id must be set");

                var entity = await _appUserManager.FindByIdAsync(request.Id.Value.ToString());

                if (entity is null)
                    throw new DataNotFoundException("User not found");

                entity = _mapper.Map<UserRequest, UserApp>(request, entity);
                entity.Image = await _storageService.ReplaceFileAsync(request.Image, "/Project_Structure/Project_Structure/wwwroot/Image/User", entity.Image);
                var result = await _appUserManager.UpdateAsync(entity);

                if (!result.Succeeded)
                    throw new DataValidationException(result.Errors.First().Description);
                
                var userRoles = (await _appUserManager.GetRolesAsync(entity)).FirstOrDefault();
                if ( userRoles != request.Role)
                {
                    await _appUserManager.RemoveFromRoleAsync(entity, request.Role);


                    result = await _appUserManager.AddToRoleAsync(entity, request.Role);

                    if (!result.Succeeded)
                        throw new DataValidationException(result.Errors.First().Description);
                }

                await transaction.CommitAsync();
            }

        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _appUserManager.FindByIdAsync(id.ToString());


            if (entity is null)
                throw new DataNotFoundException("The provided entity is not found");

            entity.IsActive = false;
            var result = await _appUserManager.UpdateAsync(entity);
            if (!result.Succeeded)
                throw new DataValidationException(result.Errors.First().Description);
        }

        public async Task<TokenResponse> Login(LoginRequeste request)
        {
            
            UserApp user;
            if (Regex.IsMatch(request.UserName,"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
            {
                user = await _appUserManager.FindByEmailAsync(request.UserName);
            }
            else
            {
                user = await _appUserManager.FindByNameAsync(request.UserName);
            }
                
            if (user is null || user.IsActive == false)
                throw new NotAuthorizedException("Invalid login attempt");

            var password = request.Password.ToString();
            var result = await _appSignInManager.CheckPasswordSignInAsync(user, password, true);
            if (!result.Succeeded)
                throw new NotAuthorizedException("Invalid login attempt");

            var token = await _appSignInManager.GenerateUserTokens(user);
            return token;
        } 
        
        public async Task ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _appUserManager.FindByIdAsync(request.UserId.ToString());

            if(user is null)
                throw new DataNotFoundException("User not found");

            var result = await _appUserManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
                throw new DataValidationException(result.Errors.First().Description);
        }
    }
}
