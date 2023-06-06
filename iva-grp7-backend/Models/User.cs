using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace iva_grp7_backend.Models;

	public class User
	{
		[Key]
		public string Id { get; set; }
		public string Avatar { get; set; }
		public Role Role { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
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

		public User()
		{
			Followers = new List<string>();
			Following = new List<string>();
			LikedPosts = new List<string>();
			DislikedPosts = new List<string>();
			CreatedAt = DateTime.UtcNow;
		}
		
}
public enum Role
{
	Admin = 0,
    
	Moderator = 1,
    
	User = 2
}

public enum Gender
{
	Male = 0,
	Female = 1,
	Diverse = 2
}

public class Follower
{
	public int Id { get; set; }
	public string FollowerUserId { get; set; } // Benutzer-ID des Followers
	public ApplicationUser User { get; set; } // Navigationseigenschaft zum Benutzer
}

public class Following
{
	public int? Id { get; set; }
	public string? FollowingUserId { get; set; } // Benutzer-ID des Following
	public ApplicationUser? User { get; set; } // Navigationseigenschaft zum Benutzer
}
