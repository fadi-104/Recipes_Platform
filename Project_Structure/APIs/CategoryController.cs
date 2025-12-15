using Azure.Core;
using BusinessLogicLayer.Services.CategoryService;
using Core.Model;
using DomainLayer.Requests;
using DomainLayer.Responses;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_Structure.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            return Ok(ApiResponse<List<CategoryResponse>>.SuccessResult(response));
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _categoryService.GetAsync(id);
            return Ok(ApiResponse<CategoryResponse>.SuccessResult(response));
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryRequest request)
        {
            await _categoryService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }

        // PUT api/<CategoryController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CategoryRequest request)
        {
            await _categoryService.UpdateAsync(request);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }
    }
}
