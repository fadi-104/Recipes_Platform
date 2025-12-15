using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Requests
{
    public class ImageRequest
    {
        public int? Id {  get; set; }
        [Required]
        public int RecipecId { get; set; }
        [Required]
        public List<IFormFile> Image {  get; set; }
    }
}
