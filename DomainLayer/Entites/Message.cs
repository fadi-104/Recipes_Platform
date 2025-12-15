using System.ComponentModel.DataAnnotations.Schema;


namespace DomainLayer.Entites
{
    public class Message : BaseEntity
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }

        [ForeignKey(nameof(SenderId))]
        public UserApp Sender { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public UserApp Receiver { get; set; }

    }
}
