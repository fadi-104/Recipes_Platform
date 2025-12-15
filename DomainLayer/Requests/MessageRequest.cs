
namespace DomainLayer.Requests
{
    public class MessageRequest
    {
        public int? Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
    }
}
