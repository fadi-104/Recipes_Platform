using BusinessLogicLayer.Services.AiService;
using Core.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_Structure.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;
        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }
        

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] List<string> prompt)
        {
            
            var response = await _aiService.GetAiResponseAsync(prompt);
            return Ok(ApiResponse<string>.SuccessResult(response));
        }

        [HttpPost]
        public async Task<IActionResult> PostN8n([FromForm] string message)
        {
            var response = await _aiService.SendToN8N(message);
            return Ok(ApiResponse<string>.SuccessResult(response));
        }

    }
}
