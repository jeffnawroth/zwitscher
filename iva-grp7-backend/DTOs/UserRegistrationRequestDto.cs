using System.ComponentModel.DataAnnotations;
using iva_grp7_backend.Models;

namespace iva_grp7_backend.Controllers
{
    public class UserRegistrationRequestDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }

}
