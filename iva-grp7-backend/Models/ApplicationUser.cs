using System;
using Microsoft.AspNetCore.Identity;

namespace iva_grp7_backend.Models
{
	public class ApplicationUser: IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		//public string Token;
		//public string Email;
		public string Role { get; set; }
		
	}
}

