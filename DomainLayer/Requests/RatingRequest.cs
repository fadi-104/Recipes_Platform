using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Requests
{
    public class RatingRequest
    {
        public int? Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public int RecipecId { get; set; }
        [Required]
        [Range(0,5)]
        public float Rate { get; set; }
    }
}
