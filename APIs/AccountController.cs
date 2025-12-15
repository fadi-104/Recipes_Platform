using BusinessLogicLayer.Services.UserService;
using Core.Model;
using DomainLayer.Requests;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_Structure.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequeste requeste)
        {
           var response = await _userService.Login(requeste);
            return Ok(ApiResponse<TokenResponse>.SuccessResult(response));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            await _userService.ChangePasswordAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }

    }
}
