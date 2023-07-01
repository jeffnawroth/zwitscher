using App3.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace App3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Beispiel-Posting erstellen und hinzufügen
            var examplePosts = new List<Postin>
            {
                new Postin
                {
                    Avatar = "avatar1.png",
                    Name = "John Doe",
                    Username = "@johndoe",
                    PostText = "Dies ist ein Beispiel-Posting",
                    Likes = 10,
                    Dislikes = 2,
                    CommentCount = 5,
                    Timestamp = DateTime.Now
                },
                 new Postin
                {
                    Avatar = "avatar2.png",
                    Name = "Jane Smith",
                    Username = "@janesmith",
                    PostText = "Ein weiteres Beispiel-Posting",
                    Likes = 15,
                    Dislikes = 3,
                    CommentCount = 8,
                    Timestamp = DateTime.Now.AddMinutes(-30)
                },
                new Postin
                { 
                    Avatar = "avatar3.png",
                    Name = "Max Mustermann",
                    Username = "@maxmustermann",
                    PostText = "Noch ein Beispiel-Posting",
                    Likes = 5,
                    Dislikes = 1,
                    CommentCount = 3,
                    Timestamp = DateTime.Now.AddHours(-1)
                }
               };

            foreach (var post in examplePosts)
            {
                AddPostToLayout(post);
            }
        }

        private void AddPostToLayout(Postin post)
        {
            var postLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Padding = new Thickness(10),
                BackgroundColor = Color.LightGray
            };

            var userLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10
            };

            var avatarImage = new Image
            {
                Source = post.Avatar,
                WidthRequest = 40,
                HeightRequest = 40,
                Aspect = Aspect.AspectFill
            };

            var userInfoLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            var nameLabel = new Label
            {
                Text = post.Name,
                FontSize = 16,
                TextColor = Color.Black
            };

            var usernameTimestampLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10
            };

            var usernameLabel = new Label
            {
                Text = post.Username,
                FontSize = 14,
                TextColor = Color.Black
            };

            var timestampLabel = new Label
            {
                Text = post.Timestamp.ToString("d MMM"),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                TextColor = Color.Black
            };

            userInfoLayout.Children.Add(nameLabel);
            userInfoLayout.Children.Add(usernameTimestampLayout);

            usernameTimestampLayout.Children.Add(usernameLabel);
            usernameTimestampLayout.Children.Add(timestampLabel);

            userLayout.Children.Add(avatarImage);
            userLayout.Children.Add(userInfoLayout);

            var postTextLabel = new Label
            {
                Text = post.PostText,
                Margin = new Thickness(0, 10, 0, 0),
                TextColor = Color.Black
            };

            var likesDislikesLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Margin = new Thickness(0, 10, 0, 0)
            };

            var likesIcon = new Label
            {
                Text = "\uf164",
                FontFamily = "FontAwesome",
                FontSize = 20,
                TextColor = Color.Black
            };

            var likesLabel = new Label
            {
                Text = post.Likes.ToString(),
                TextColor = Color.Black
            };

            var dislikesIcon = new Label
            {
                Text = "\uf165",
                FontFamily = "FontAwesome",
                FontSize = 20,
                TextColor = Color.Black
            };

            var dislikesLabel = new Label
            {
                Text = post.Dislikes.ToString(),
                TextColor = Color.Black
            };

            var commentCountLabel = new Label
            {
                Text = "Kommentare: " + post.CommentCount.ToString(),
                TextColor = Color.Black
            };

            likesDislikesLayout.Children.Add(likesIcon);
            likesDislikesLayout.Children.Add(likesLabel);
            likesDislikesLayout.Children.Add(dislikesIcon);
            likesDislikesLayout.Children.Add(dislikesLabel);
            likesDislikesLayout.Children.Add(commentCountLabel);

            postLayout.Children.Add(userLayout);
            postLayout.Children.Add(postTextLabel);
            postLayout.Children.Add(likesDislikesLayout);

            mainLayout.Children.Add(postLayout);
        }
    }

    public class Postin
    {
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PostText { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int CommentCount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
