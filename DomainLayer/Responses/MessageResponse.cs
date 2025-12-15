
namespace DomainLayer.Responses
{
    public class MessageResponse
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
