using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace iva_grp7_backend.Models;

	public class User
	{
		public string Id { get; set; }
		public string Avatar { get; set; }
		public Role Role { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public Gender? Gender { get; set; }
		public DateTime? BirthDate { get; set; }
		public List<Follower> Followers { get; set; }
		public List<Following> Following { get; set; }
        public List<Post> LikedPosts { get; set; }
        //[ForeignKey("LikedByUserId")]
        public List<Post> DislikedPosts { get; set; }
        //[ForeignKey("DislikedByUserId")]
		public DateTime CreatedAt { get; set; }
		public string? Bio { get; set; }
		public List<Interest> Interests { get; set; }
		public bool Locked { get; set; }
		
		// Jedem User werden seine eigenen Posts zugeordnet
		public ICollection<Post> Posts { get; set; }

		
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
