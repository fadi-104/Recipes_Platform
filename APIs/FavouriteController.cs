using Azure;
using Azure.Core;
using BusinessLogicLayer.Services.FavouriteService;
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
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;

        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }


        // GET: api/<FavouriteController>
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(int userId)
        {
            var response = await _favouriteService.GetFavouriteAsync(userId);
            return Ok(ApiResponse<List<RecipecResponse>>.SuccessResult(response));
        }

        // POST api/<FavouriteController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FavouriteRequest request)
        {
            await _favouriteService.AddFavouriteAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }

        // DELETE api/<FavouriteController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _favouriteService.RemoveFavouriteAsync(id);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }
    }
}
