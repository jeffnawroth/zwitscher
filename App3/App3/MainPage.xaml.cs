using App3.Layouts;
using App3.Services;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace App3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {

            var user = GetUser();
            var postsStackLayout = new StackLayout();

            var dummyPosts = DummyPost.CreateDummyPosts();

            foreach (var post in dummyPosts)
            {
                var postLayout = PostLayout.CreatePostLayout(post, user, currentUserId);
                postsStackLayout.Children.Add(postLayout);
            }

            var scrollView = new ScrollView
            {
                Content = postsStackLayout
            };

            Content = scrollView;
        }

        private int currentUserId = 1;
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
