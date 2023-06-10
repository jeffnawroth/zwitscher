using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace iva_grp7_backend.Models
{
	public class Post
    {
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string UserId { get; set; }
		[NotMapped]
		public string? Avatar { get; set; }
		[NotMapped]
		public string Name { get; set; }
		[NotMapped]
        public string Username { get; set; }
        public string? Text { get; set; }
        public int UpVotes { get; set; }
		public int DownVotes { get; set; }
		public string Date { get; set; }
		[NotMapped]
		public List<string>? Comments { get; set; }
		[NotMapped]
		public List<string>? Files { get; set; }
		public Post()
		{
			Date = DateTime.UtcNow.ToString();
		}
    }
}

