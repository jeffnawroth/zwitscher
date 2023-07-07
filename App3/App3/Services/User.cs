using System;
using System.Collections.Generic;

namespace App3.Services
{
    public class User
    {
        public string Id { get; set; }
        public string Avatar { get; set; }
        public Role Role { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender? Gender { get; set; }
        public List<string> Followers { get; set; }
        public List<string> Following { get; set; }
        public string Bio { get; set; }
        //public List<int> LikedPosts { get; set; }
        //public List<int> DislikedPosts { get; set; }
        public DateTime CreatedAt { get; set; }
        public string BirthDate { get; set; }
        public List<string> Interests { get; set; }
        public bool Locked { get; set; }
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

   
        // Weitere Eigenschaften für Dateiinformationen

        // Eigenschaft "file" für den Avatar
    

}
