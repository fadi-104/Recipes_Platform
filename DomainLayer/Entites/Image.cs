using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer.Entites
{
    public class Image : BaseEntity
    {
        public int RecipecId { get; set; }
        public string ImageUrl { get; set; }

        [ForeignKey(nameof(RecipecId))]
        public Recipec Recipec { get; set; }
    }
}
