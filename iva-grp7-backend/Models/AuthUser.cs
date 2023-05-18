using System;
using Microsoft.AspNetCore.Identity;

namespace iva_grp7_backend.Models
{
	public class AuthUser: IdentityUser
	{
		public string FirstName;
		public string LastName;
		public string Token;
		public string Role;
	}
}

