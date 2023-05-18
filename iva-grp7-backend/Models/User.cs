using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace iva_grp7_backend.Models;

	public class User: IUser
	{
		
		public int Id { get; set; }
		public string Avatar { get; set; }
		public Role Role { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public Gender Gender { get; set; }
		public DateTime BirthDate { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Following { get; set; }
        public List<Post> LikedPosts { get; set; }
        [ForeignKey("LikedByUserId")]
        public List<Post> DislikedPosts { get; set; }
        [ForeignKey("DislikedByUserId")]
		public DateTime CreatedAt { get; set; }
		public string Bio { get; set; }
		public List<string> Interests { get; set; }

}

