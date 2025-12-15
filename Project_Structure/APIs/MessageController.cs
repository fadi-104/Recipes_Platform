using BusinessLogicLayer.Services.MessageService;
using Core.Model;
using DomainLayer.Responses;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_Structure.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] MessageOption option)
        {
            var messages = await _messageService.GetAllAsync(option.Skip, option.PageSize, option.SenderId, option.ReceiverId);

            var totalPages = (int)Math.Ceiling((double)messages.TotalCount / option.PageSize);
            var fromItem = option.Skip + 1;
            var toItem = option.Skip + option.PageSize;

            var response = new PagedResponse<List<MessageResponse>>
            {
                Data = messages.Data,
                TotalCount = messages.TotalCount,
                TotalPages = totalPages,
                FromItems = fromItem,
                ToItems = toItem > messages.TotalCount ? messages.TotalCount : toItem,
                PageSize = option.PageSize
            };

            return Ok(ApiResponse<PagedResponse<List<MessageResponse>>>.SuccessResult(response));
        }

        // GET api/<MessageController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _messageService.GetAsync(id);
            return Ok(ApiResponse<MessageResponse>.SuccessResult(response));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _messageService.DeleteAsync(id);
            return Ok(ApiResponse<object>.SuccessResult(null));
        }

    }
}
