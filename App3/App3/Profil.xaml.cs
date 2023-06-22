using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App3.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3
{
    public partial class Profil : ContentPage
    {
        private User currentUser;

        public Profil()
        {
            InitializeComponent();
        }

        public Profil(User user) : this()
        {
            currentUser = user;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Set the profile information
            ProfileNameLabel.Text = currentUser.Name;
            UsernameLabel.Text = $"@{currentUser.Username}";
            FollowersLabel.Text = $"{currentUser.Followers.Count} Abonnenten";
            FollowingLabel.Text = $"{currentUser.Following.Count} Folge ich";

            // Set the user avatar
            if (currentUser.Avatar != null)
            {
                AvatarImage.Source = currentUser.Avatar.Path;  // Beispiel: Verwende die Path-Eigenschaft des AvatarFile-Objekts
            }
            else
            {
                AvatarImage.Source = "placeholder_avatar.png";
            }

            // Load and display user's posts
            LoadUserPosts();
        }

        private void LoadUserPosts()
        {
            // Get the user's posts from the database or API
            var userPosts = GetUserPosts(currentUser.Id);

            // Clear the existing post views
            PostListLayout.Children.Clear();

            // Create and add a PostView for each post
            foreach (var post in userPosts)
            {
                var postView = new PostView(post);
                PostListLayout.Children.Add(postView);
            }
        }

        private List<Post> GetUserPosts(int userId)
        {
            // Logic to fetch the user's posts from the database or API
            // Replace with your own implementation
            // This is just a dummy implementation
            var posts = new List<Post>
            {
                new Post { PostId = 1, UserId = userId, Content = "Post 1" },
                new Post { PostId = 2, UserId = userId, Content = "Post 2" },
                new Post { PostId = 3, UserId = userId, Content = "Post 3" }
            };

            return posts;
        }
    }
}
