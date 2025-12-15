using BusinessLogicLayer.Services.RatingService;
using Core.Model;
using DomainLayer.Requests;
using DomainLayer.Responses;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_Structure.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // GET: api/<RatingController>
        [HttpGet]
        public async Task<IActionResult> Get(int recipecId, int userId)
        {
            var response = await _ratingService.GetAsync(recipecId, userId);
            return Ok(ApiResponse<RateResponse>.SuccessResult(response));
        }


        // POST api/<RatingController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RatingRequest request)
        {
            await _ratingService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }

        // PUT api/<RatingController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] RatingRequest request)
        {
            await _ratingService.UpdateAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }

    }
}
