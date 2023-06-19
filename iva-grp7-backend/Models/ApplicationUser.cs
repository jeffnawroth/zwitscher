using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace iva_grp7_backend.Models
{
	public class ApplicationUser : IdentityUser
    {
        private string _email;
        public byte[]? Avatar { get; set; }
        public Role Role { get; set; } 
        public string Name { get; set; }
        public Gender? Gender { get; set; }
        public string? BirthDate { get; set; }
        public List<UserFollower> Followers { get; set; }
        public List<UserFollowing> Following { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Bio { get; set; }
        public List<UserInterest>? Interests { get; set; }
        public bool Locked { get; set; }
        
        public ApplicationUser()
        {
            CreatedAt = DateTime.UtcNow;
        }
        
        public override string Email
        {
            get => _email;
            set
            {
                _email = value;
                NormalizedEmail = _email?.ToUpperInvariant().Normalize();
            }
        }
    }

	
	
}

