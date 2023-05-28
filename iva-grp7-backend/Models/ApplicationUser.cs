using System;
using Microsoft.AspNetCore.Identity;

namespace iva_grp7_backend.Models
{
	public class ApplicationUser: IdentityUser
	{
		public string Name { get; set; }
		public string? Avatar { get; set; }
		public Role Role { get; set; }
		public Gender? Gender { get; set; }
		public DateTime? BirthDate { get; set; }
		public List<Follower> Followers { get; set; }
		public List<Following> Following { get; set; }
		public List<Post> LikedPosts { get; set; }
		public List<Post> DislikedPosts { get; set; }
		public DateTime CreatedAt { get; set; }
		public string? Bio { get; set; }
		public List<Interest>? Interests { get; set; }
		public bool Locked { get; set; }
		// Jedem User werden seine eigenen Posts zugeordnet
		public ICollection<Post> Posts { get; set; }
		
		public ApplicationUser(){
			// Initialisiere die Liste der Follower mit einer leeren Liste
			Followers = new List<Follower>();
			Following = new List<Following>();
			CreatedAt = DateTime.UtcNow;
		}
	}
	
	
}

