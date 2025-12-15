using System.ComponentModel.DataAnnotations;


namespace DomainLayer.Requests
{
    public class CategoryRequest
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
