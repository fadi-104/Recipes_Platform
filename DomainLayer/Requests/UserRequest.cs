using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace DomainLayer.Requests
{
    public class UserRequest
    {
        public  int? Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        
        [Required]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public string ComfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"^(?:\+963|09)[0-9]{8}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [Range(13,100)]
        public short Age { get; set; }
        [Required]
        public string Role { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public bool IsActive { get; set; }

    }
}
