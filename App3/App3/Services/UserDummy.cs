using System;
using System.Collections.Generic;
using System.Text;

using App3.Services;

namespace App3.Services
{
    public static class UserDummy
    {
        public static List<User> GetDummyUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Avatar = new AvatarFile { /* Dateiinformationen */ },
                    Role = Role.User,
                    Username = "user1",
                    Name = "Dummy User 1",
                    Email = "user1@example.com",
                    Password = "password",
                    Gender = Gender.Male,
                    Followers = new List<string> { /* Liste von Follower-IDs */ },
                    Following = new List<string> { /* Liste von Following-IDs */ },
                    Bio = "Hello, I'm Dummy User 1!",
                    LikedPosts = new List<string> { /* Liste von Post-IDs */ },
                    DislikedPosts = new List<string> { /* Liste von Post-IDs */ },
                    CreatedAt = DateTime.Now,
                    BirthDate = new DateTime(1990, 1, 1),
                    Interests = new List<string> { /* Liste von Interessen */ },
                    Locked = false
                },
                new User
                {
                    Id = 2,
                    Avatar = new AvatarFile { Path = "placeholder_avatar" },
                    Role = Role.User,
                    Username = "user2",
                    Name = "Dummy User 2",
                    Email = "user2@example.com",
                    Password = "1234",
                    Gender = Gender.Female,
                    Followers = new List<string> { /* Liste von Follower-IDs */ },
                    Following = new List<string> { /* Liste von Following-IDs */ },
                    Bio = "Hello, I'm Dummy User 2!",
                    LikedPosts = new List<string> { /* Liste von Post-IDs */ },
                    DislikedPosts = new List<string> { /* Liste von Post-IDs */ },
                    CreatedAt = DateTime.Now,
                    BirthDate = new DateTime(1995, 5, 10),
                    Interests = new List<string> { /* Liste von Interessen */ },
                    Locked = false
                }
                // Füge weitere Dummy-Nutzer hinzu...
            };

            return users;
        }
    }
}
