using BusinessLogicLayer.Services.MessageService;
using DomainLayer.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Project_Structure.ChatHub
{

    [Authorize]
    public sealed class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} joined the chat", true);
        }

        public async Task SendMessage(int userId,string message)
        {
            var request = new MessageRequest
            {
                SenderId = int.Parse(Context.UserIdentifier),
                ReceiverId = userId,
                Content = message
            };

            await _messageService.CreateAsync(request);

            var name = Context.User?.Identity?.Name.ToString();
            await Clients.User(userId.ToString()).SendAsync("ReceiveMessage", $"{name}", message, false);

        }
    }
}
