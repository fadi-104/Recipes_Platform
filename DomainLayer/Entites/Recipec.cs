using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainLayer.Entites
{
    public class Recipec : BaseEntity
    {
        [MaxLength(30)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int ChefId { get; set; }

        [MaxLength(60)]
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Steps { get; set; }
        public int CookTime { get; set; }
        public string BaseImage { get; set; }
        public bool IsPublished { get; set; }

        [DefaultValue(0)]
        public int ViewCount { get; set; }

        [ForeignKey(nameof(ChefId))]
        public UserApp Chef { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}
