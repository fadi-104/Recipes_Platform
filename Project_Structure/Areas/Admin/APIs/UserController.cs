using BusinessLogicLayer.Services.UserService;
using Core.Model;
using DomainLayer.Requests;
using DomainLayer.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_Structure.Areas.Admin.APIs
{
    [Area("Admin")]
    [Route("[area]/api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] TableOptions options, string role, bool? isActive)
        {
            var data = await _userService.GetAllAsync(options, role, isActive);
            var totalPages = (int)Math.Ceiling((double)data.TotalCount / options.PageSize);
            var forItem = options.Skip + 1;
            var toItem = options.Skip + options.PageSize;

            var response =  new PagedResponse<List<UserResponse>>
            {
                Data = data.Data,
                TotalCount = data.TotalCount,
                TotalPages = totalPages,
                FromItems = forItem,
                ToItems = toItem > data.TotalCount ? data.TotalCount : toItem,
                PageSize = options.PageSize
            };

            return Ok(ApiResponse<PagedResponse<List<UserResponse>>>.SuccessResult(response));
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _userService.GetAsync(id);
            return Ok(ApiResponse<UserResponse>.SuccessResult(response));
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            var response = await _userService.GetByUserNameAsync(userName);
            return Ok(ApiResponse<UserResponse>.SuccessResult(response));
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] UserRequest request)
        {
            await _userService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));

        }

        // PUT api/<UserController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] UserRequest request)
        {
            await _userService.UpdateAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }
    }
}
