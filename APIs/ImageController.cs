using Azure.Core;
using BusinessLogicLayer.Services.ImageService;
using Core.Model;
using DomainLayer.Entites;
using DomainLayer.Requests;
using DomainLayer.Responses;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_Structure.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        // GET: api/<ImageController>
        [HttpGet]
        public async Task<IActionResult> GetAll(int recipecId)
        {
            var resonse = await _imageService.GetAllAsync(recipecId);
            return Ok(ApiResponse<List<ImageResponse>>.SuccessResult(resonse));
        }

        // GET api/<ImageController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var resonse = await _imageService.GetByIdAsync(id);
            return Ok(ApiResponse<ImageResponse>.SuccessResult(resonse));
        }

        // POST api/<ImageController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ImageRequest request)
        {
            await _imageService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }


        // DELETE api/<ImageController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _imageService.DeleteAsync(id);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }
    }
}
