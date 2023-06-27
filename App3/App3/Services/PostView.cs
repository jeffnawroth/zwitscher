using App3.Services;
using Xamarin.Forms;
using System;

namespace App3.Services
{
    public class PostView : ContentView
    {
        private Post post;

        public PostView(Post post)
        {
            this.post = post;
            Display();
        }

        public void Display()
        {
            var userLabel = new Label
            {
                Text = post.UserName,
                FontAttributes = FontAttributes.Bold
            };

            var avatarImage = new Image
            {
                Source = post.UserAvatar
            };

            var contentLabel = new Label
            {
                Text = post.Content
            };

            var timestampLabel = new Label
            {
                Text = post.Timestamp.ToString()
            };

            var thumbsUpImage = new Image
            {
                Source = "thumbs.png"
            };

            var thumbsDownImage = new Image
            {
                Source = "thumbs_down_icon.png"
            };

            var thumbsUpLabel = new Label
            {
                Text = post.ThumbsUpUserIds.Count.ToString()
            };

            var thumbsDownLabel = new Label
            {
                Text = post.ThumbsDownUserIds.Count.ToString()
            };

            var postLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            avatarImage,
                            userLabel
                        }
                    },
                    contentLabel,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            timestampLabel,
                            thumbsUpImage,
                            thumbsUpLabel,
                            thumbsDownImage,
                            thumbsDownLabel
                        }
                    }
                }
            };

            Content = postLayout;
        }
    }
}
