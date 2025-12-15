using BusinessLogicLayer.Services.RecipecService;
using Core.Model;
using DomainLayer.Requests;
using DomainLayer.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_Structure.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecipecController : ControllerBase
    {
        private readonly IRecipecService _recipecService;
        public RecipecController(IRecipecService recipecService)
        {
            _recipecService = recipecService;
        }

        // GET: api/<RecipecController>
        [HttpPost]
        public async Task<IActionResult> GetAll(TableOptions options, string? name, bool? isPublished, int? categoryId, DateTime? date)
        {
            var list = await _recipecService.GetAllAsync(options, name, isPublished, categoryId, date);

            var totalPages = (int)Math.Ceiling((double)list.TotalCount / options.PageSize);
            var fromItem = options.Skip + 1;
            var toItem = options.Skip + options.PageSize;

            var response = new PagedResponse<List<DomainLayer.Responses.RecipecResponse>>
            {
                Data = list.Data,
                TotalCount = list.TotalCount,
                TotalPages = totalPages,
                FromItems = fromItem,
                ToItems = toItem > list.TotalCount ? list.TotalCount : toItem,
                PageSize = options.PageSize
            };

            return Ok(ApiResponse<PagedResponse<List<RecipecResponse>>>.SuccessResult(response));

        }

        // GET api/<RecipecController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var key = User.Identity?.Name ?? HttpContext.Connection.RemoteIpAddress?.ToString();
            var response = await _recipecService.GetAsync(id,key);
            return Ok(ApiResponse<RecipecResponse>.SuccessResult(response));
        }

        // POST api/<RecipecController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] RecipecRequest request)
        {
            await _recipecService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }

        // PUT api/<RecipecController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] RecipecRequest request)
        {
            await _recipecService.UpdateAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }

        // DELETE api/<RecipecController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _recipecService.DeleteAsync(id);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }
    }
}
