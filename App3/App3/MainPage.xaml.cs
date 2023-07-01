using App3.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace App3
{
    public partial class MainPage : ContentPage
    {
        private int currentUserId = 123; // Beispielwert für die aktuelle Benutzer-ID

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var examplePosts = new List<Postin>
            {
                // Beispieldaten
                new Postin
                {
                    PostId = 1,
                    Avatar = "placeholder_avatar.png",
                    Name = "John Doe",
                    UserId = 1,
                    Username = "@johndoe",
                    PostText = "Dies ist Beispiel-Post 1",
                    Likes = new List<int> { 1, 2, 3 },
                    Dislikes = new List<int> { 4, 5 },
                    CommentCount = new List<int> { 6, 7, 8 },
                    Timestamp = DateTime.Now
                },
                new Postin
                {
                    PostId = 2,
                    Avatar = "placeholder_avatar.png",
                    Name = "Jane Smith",
                    UserId = 2,
                    Username = "@janesmith",
                    PostText = "Dies ist Beispiel-Post 2",
                    Likes = new List<int> { 9, 10 },
                    Dislikes = new List<int> { 11 },
                    CommentCount = new List<int> { 12, 13 },
                    Timestamp = DateTime.Now.AddDays(-1)
                },
                new Postin
                {
                    PostId = 11,
                    Avatar = "placeholder_avatar.png",
                    Name = "Laura Miller",
                    UserId = 11,
                    Username = "@lauramiller",
                    PostText = "Dies ist Beispiel-Post 11",
                    Likes = new List<int> { 61, 62 },
                    Dislikes = new List<int> { 63, 64 },
                    CommentCount = new List<int> { 65, 66 },
                    Timestamp = DateTime.Now.AddDays(-10)
                },
                new Postin
                {
                    PostId = 12,
                    Avatar = "placeholder_avatar.png",
                    Name = "Andrew Thompson",
                    UserId = 12,
                    Username = "@andrewthompson",
                    PostText = "Dies ist Beispiel-Post 12",
                    Likes = new List<int> { 67, 68 },
                    Dislikes = new List<int> { 69 },
                    CommentCount = new List<int> { 70, 71, 72 },
                    Timestamp = DateTime.Now.AddDays(-11)
                },
                new Postin
                {
                    PostId = 13,
                    Avatar = "placeholder_avatar.png",
                    Name = "Emma Wilson",
                    UserId = 13,
                    Username = "@emmawilson",
                    PostText = "Dies ist Beispiel-Post 13",
                    Likes = new List<int> { 73 },
                    Dislikes = new List<int> { 74, 75 },
                    CommentCount = new List<int> { 76, 77 },
                    Timestamp = DateTime.Now.AddDays(-12)
                },
                new Postin
                {
                    PostId = 14,
                    Avatar = "placeholder_avatar.png",
                    Name = "Jacob Davis",
                    UserId = 14,
                    Username = "@jacobdavis",
                    PostText = "Dies ist Beispiel-Post 14",
                    Likes = new List<int> { 78, 79 },
                    Dislikes = new List<int> { 80 },
                    CommentCount = new List<int> { 81, 82, 83 },
                    Timestamp = DateTime.Now.AddDays(-13)
                },
                new Postin
                {
                    PostId = 15,
                    Avatar = "placeholder_avatar.png",
                    Name = "Olivia Moore",
                    UserId = 15,
                    Username = "@oliviamoore",
                    PostText = "Dies ist Beispiel-Post 15",
                    Likes = new List<int> { 84, 85 },
                    Dislikes = new List<int> { 86, 87 },
                    CommentCount = new List<int> { 88, 89 },
                    Timestamp = DateTime.Now.AddDays(-14)
                },
                new Postin
                {
                    PostId = 16,
                    Avatar = "placeholder_avatar.png",
                    Name = "David Taylor",
                    UserId = 16,
                    Username = "@davidtaylor",
                    PostText = "Dies ist Beispiel-Post 16",
                    Likes = new List<int> { 90, 91 },
                    Dislikes = new List<int> { 92 },
                    CommentCount = new List<int> { 93, 94, 95 },
                    Timestamp = DateTime.Now.AddDays(-15)
                },
                new Postin
                {
                    PostId = 17,
                    Avatar = "placeholder_avatar.png",
                    Name = "Sophie Clark",
                    UserId = 17,
                    Username = "@sophieclark",
                    PostText = "Dies ist Beispiel-Post 17",
                    Likes = new List<int> { 96, 97, 98 },
                    Dislikes = new List<int> { 99, 100 },
                    CommentCount = new List<int> { 101 },
                    Timestamp = DateTime.Now.AddDays(-16)
                },
                new Postin
                {
                    PostId = 18,
                    Avatar = "placeholder_avatar.png",
                    Name = "William Harris",
                    UserId = 18,
                    Username = "@williamharris",
                    PostText = "Dies ist Beispiel-Post 18",
                    Likes = new List<int> { 102, 103 },
                    Dislikes = new List<int> { 104, 105 },
                    CommentCount = new List<int> { 106, 107 },
                    Timestamp = DateTime.Now.AddDays(-17)
                },
                new Postin
                {
                    PostId = 19,
                    Avatar = "placeholder_avatar.png",
                    Name = "Emily Rodriguez",
                    UserId = 19,
                    Username = "@emilyrodriguez",
                    PostText = "Dies ist Beispiel-Post 19",
                    Likes = new List<int> { 108 },
                    Dislikes = new List<int> { 109, 110 },
                    CommentCount = new List<int> { 111, 112 },
                    Timestamp = DateTime.Now.AddDays(-18)
                },
                new Postin
                {
                    PostId = 20,
                    Avatar = "placeholder_avatar.png",
                    Name = "Alexander White",
                    UserId = 20,
                    Username = "@alexanderwhite",
                    PostText = "Dies ist Beispiel-Post 20",
                    Likes = new List<int> { 113, 114 },
                    Dislikes = new List<int> { 115 },
                    CommentCount = new List<int> { 116, 117, 118 },
                    Timestamp = DateTime.Now.AddDays(-19)
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
                BackgroundColor = Color.White,
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
                Spacing = 10
            };

            var likesIcon = new Button
            {
                ImageSource = "thum_up.png",
                FontSize = 0.00000001,
                TextColor = Color.Default,
                
            };

            var likesLabel = new Label
            {
                Text = post.Likes.Count.ToString(),
                TextColor = Color.Black
            };


            var dislikesIcon = new Button
            {
                ImageSource = "thumb_down.png", 
                FontSize= 0.01,
                BackgroundColor = Color.Default
            };

            var dislikesLabel = new Label
            {
                Text = post.Dislikes.Count.ToString(),
                TextColor = Color.Black
            };
            likesIcon.Clicked += (s, e) =>
            {
                if (post.Likes.Contains(currentUserId))
                {
                    post.Likes.Remove(currentUserId);
                    likesIcon.ImageSource = "like_pushed.png";
                }
                else
                {
                    post.Likes.Add(currentUserId);
                    likesIcon.BackgroundColor = Color.DarkGray;

                   
                    post.Dislikes.Remove(currentUserId);
                    dislikesIcon.ImageSource = "thum_down.png";
                }

                likesLabel.Text = post.Likes.Count.ToString();
            };
     
            dislikesIcon.Clicked += (s, e) =>
            {
                if (post.Dislikes.Contains(currentUserId))
                {
                    post.Dislikes.Remove(currentUserId);
                    dislikesIcon.ImageSource = "dislike_pushed.png";
                }
                else
                {
                    post.Dislikes.Add(currentUserId);
                    dislikesIcon.BackgroundColor = Color.DarkGray;

                   
                    post.Likes.Remove(currentUserId);
                    likesIcon.ImageSource = "thumb_up";
                }

                likesLabel.Text = post.Likes.Count.ToString();
            };

            var commentsIcon = new Button
            {
                ImageSource ="comment.png",
                FontSize = 0.1,
                BackgroundColor = Color.Default
            };

            var commentsLabel = new Label
            {
                Text = post.CommentCount.Count.ToString(),
                TextColor = Color.Black
            };

            likesDislikesLayout.Children.Add(likesIcon);
            likesDislikesLayout.Children.Add(likesLabel);
            likesDislikesLayout.Children.Add(dislikesIcon);
            likesDislikesLayout.Children.Add(dislikesLabel);
            likesDislikesLayout.Children.Add(commentsIcon);
            likesDislikesLayout.Children.Add(commentsLabel);

            var comments = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 5
            };

            foreach (var comment in post.CommentCount)
            {
                var commentLabel = new Label
                {
                    Text = comment.ToString(),
                    TextColor = Color.Black
                };

                comments.Children.Add(commentLabel);
            }

            postLayout.Children.Add(userLayout);
            postLayout.Children.Add(postTextLabel);
            postLayout.Children.Add(likesDislikesLayout);
            postLayout.Children.Add(comments);
            mainLayout.Children.Add(postLayout);
        }
    }

    public class Postin
    {
        public int PostId { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PostText { get; set; }
        public List<int> Likes { get; set; }
        public List<int> Dislikes { get; set; }
        public List<int> CommentCount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
