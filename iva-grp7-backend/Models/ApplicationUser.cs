using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace iva_grp7_backend.Models
{
	public class ApplicationUser: IdentityUser
	{
		public string Name { get; set; }
		//public string Username { get; set; }
		public string? Avatar { get; set; }
		public Role Role { get; set; }
		public Gender? Gender { get; set; }
		public DateTime? BirthDate { get; set; }
		[NotMapped]
		public List<string> Followers { get; set; }
		[NotMapped]
		public List<string> Following { get; set; }
		[NotMapped]
		public List<string> LikedPosts { get; set; }
		[NotMapped]
		public List<string> DislikedPosts { get; set; }
		public DateTime CreatedAt { get; set; }
		public string? Bio { get; set; }
		[NotMapped]
		public List<string>? Interests { get; set; }
		public bool Locked { get; set; }
		// Jedem User werden seine eigenen Posts zugeordnet
		//public ICollection<Post> Posts { get; set; }
		
		public ApplicationUser(){
			
			Followers = new List<string>();
			Following = new List<string>();
			LikedPosts = new List<string>();
			DislikedPosts = new List<string>();
			CreatedAt = DateTime.UtcNow;
		}
	}
	
	
}

