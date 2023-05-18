using System;
namespace iva_grp7_backend.Models
{
	public class AuthResult
	{
        public List<string> Errors { get; set; }
        public IEnumerable<string>? Errors2 { get; set; }

        // Normally AuthUserResult
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}

