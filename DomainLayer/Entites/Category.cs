using System.ComponentModel.DataAnnotations;


namespace DomainLayer.Entites
{
    public class Category : BaseEntity
    {
        [MaxLength(25)]
        public string Name { get; set; }
        
    }
}
