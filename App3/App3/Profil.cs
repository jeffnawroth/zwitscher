using App3.Layouts;
using App3.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace App3
{
    public class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            var user = GetUser(); // Hier erhalten Sie Ihre Benutzerdaten

            var profileLayout = ProfilePageLayout.CreateProfilePageLayout(user);

            Content = new StackLayout
            {
                Children = { profileLayout }
            };
        }

        private User GetUser()
        {
            // Hier können Sie Ihre Benutzerdaten abrufen oder erstellen
            // Beispiel:
            var user = new User
            {
                Id = 1,
                Avatar = "placeholder_avatar.png",
                Role = Role.User,
                Username = "user1",
                Name = "Dummy User 1",
                Email = "user1@example.com",
                Password = "password",
                Gender = Gender.Male,
                Followers = new List<int> { 2, 3, 4 },
                Following = new List<int> { 2, 4 },
                Bio = "Hello, I'm Dummy User 1!",
                LikedPosts = new List<int> { /* Liste von Post-IDs */ },
                DislikedPosts = new List<int> { /* Liste von Post-IDs */ },
                CreatedAt = DateTime.Now,
                BirthDate = new DateTime(1990, 1, 1),
                Interests = new List<string> { "schwimmen", "lesen", "Radfahren", "Musik" },
                Locked = false
            };

            return user;
        }
    }
}
