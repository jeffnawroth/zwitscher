using System;
using System.Collections.Generic;

namespace App3.Services
{
    public class User
    {
        public int Id { get; set; }
        public String Avatar { get; set; }
        public Role Role { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public List<int> Followers { get; set; }
        public List<int> Following { get; set; }
        public string Bio { get; set; }
        public List<int> LikedPosts { get; set; }
        public List<int> DislikedPosts { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime BirthDate { get; set; }
        public List<string> Interests { get; set; }
        public bool Locked { get; set; }
    }

    public enum Role
    {
        User,
        Admin
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }

   
        // Weitere Eigenschaften für Dateiinformationen

        // Eigenschaft "file" für den Avatar
    

}
