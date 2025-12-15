using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Entites
{
    public class UserApp : IdentityUser<int>
    {
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(20)]
        public string LastName { get; set; }
        public string Gender { get; set; }
        [Range(10, 60)]
        public short Age { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
    }
}
