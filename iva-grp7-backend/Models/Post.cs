using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace iva_grp7_backend.Models
{
	public class Post: IdentityUser
    {
		public string Id { get; set; }
		public string UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }
		public string? Avatar { get; set; }
		public string Name { get; set; }
        public string Username { get; set; }
        public string? Text { get; set; }
        public int UpVotes { get; set; }
		public int DownVotes { get; set; }
		public DateTime Date { get; set; }
		public List<Post>? Comments { get; set; }
		public Post()
		{
			Date = DateTime.UtcNow;
		}
    }
}

