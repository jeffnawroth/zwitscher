using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace iva_grp7_backend.Models
{
	public class Post
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }
		public string Avatar { get; set; }
		public string Firstname { get; set; }
		public string LastName { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        //public List<string> Files { get; set; }
		public int UpVotes { get; set; }
		public int DownVotes { get; set; }
		public DateTime CreatedAt { get; set; }
		public List<Post> Comments { get; set; }

		// Fremdschlüssel für DislikedByUser
		public string DislikedByUserId { get; set; }
		public User DislikedByUser { get; set; }
    
		// Fremdschlüssel für LikedByUser
		public string LikedByUserId { get; set; }
		public User LikedByUser { get; set; }
    }
}

