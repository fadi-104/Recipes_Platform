using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainLayer.Entites
{
    public class Rating : BaseEntity
    {
        public int RecipecId { get; set; }
        public int UserId { get; set; }

        [Range(0,5)]
        public float Rate { get; set; }


        [ForeignKey(nameof(UserId))]
        public UserApp User { get; set; }

        [ForeignKey(nameof(RecipecId))]
        public Recipec Recipec { get; set; }
    }
}
