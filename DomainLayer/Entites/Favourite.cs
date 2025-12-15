using System.ComponentModel.DataAnnotations.Schema;


namespace DomainLayer.Entites
{
    public class Favourite : BaseEntity
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserApp User { get; set; }

        [ForeignKey(nameof(RecipeId))]
        public Recipec Recipe { get; set; }
    }
}
