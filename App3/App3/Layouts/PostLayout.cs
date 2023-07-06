using Xamarin.Forms;
using System.Collections.Generic;
using App3.Services;
using System.Threading.Tasks;

namespace App3.Layouts
{
    public class PostLayout
    {
        public static StackLayout CreatePostLayout(Posting post, User user, int currentUserId)
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

            var avatarImage = new ImageButton
            {
                Source = post.Avatar,
                WidthRequest = 40,
                HeightRequest = 40,
                Aspect = Aspect.AspectFill
            };
            avatarImage.Clicked += async (sender, e) =>
            {
                await OpenUserProfile(user);
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
                Text = "@" + post.Username,
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
            usernameTimestampLayout.Children.Add(usernameLabel);
            usernameTimestampLayout.Children.Add(timestampLabel);
            userInfoLayout.Children.Add(usernameTimestampLayout);

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

            var likeButton = new ImageButton
            {
                Source = "thumb_up.png",
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Color.Transparent
            };

            var likesLabel = new Label
            {
                Text = post.Likes.Count.ToString(),
                TextColor = Color.Black
            };

            var dislikeButton = new ImageButton
            {
                Source = "thumb_down.png",
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Color.Transparent
            };

            var dislikesLabel = new Label
            {
                Text = post.Dislikes.Count.ToString(),
                TextColor = Color.Black
            };

            likeButton.Clicked += (s, e) =>
            {
                if (post.Likes.Contains(currentUserId))
                {
                    post.Likes.Remove(currentUserId);
                    user.LikedPosts.Remove(post.PostId);
                    likeButton.Source = "thumb_up.png";
                }
                else
                {
                    post.Likes.Add(currentUserId);
                    user.DislikedPosts.Add(post.PostId);
                    likeButton.Source = "like_pushed.png";

                    if (post.Dislikes.Contains(currentUserId))
                    {
                        post.Dislikes.Remove(currentUserId);
                        user.DislikedPosts.Remove(post.PostId);
                        dislikeButton.Source = "thumb_down.png";
                    }
                }

                likesLabel.Text = post.Likes.Count.ToString();
                dislikesLabel.Text = post.Dislikes.Count.ToString();
            };

            dislikeButton.Clicked += (s, e) =>
            {
                if (post.Dislikes.Contains(currentUserId))
                {
                    post.Dislikes.Remove(currentUserId);
                    user.DislikedPosts.Remove(post.PostId);
                    dislikeButton.Source = "thumb_down.png";
                }
                else
                {
                    post.Dislikes.Add(currentUserId);
                    user.DislikedPosts.Add(post.PostId);
                    dislikeButton.Source = "dislike_pushed.png";

                    if (post.Likes.Contains(currentUserId))
                    {
                        post.Likes.Remove(currentUserId);
                        user.LikedPosts.Remove(post.PostId);
                        likeButton.Source = "thumb_up.png";
                    }
                }

                likesLabel.Text = post.Likes.Count.ToString();
                dislikesLabel.Text = post.Dislikes.Count.ToString();
            };

            var commentsButton = new ImageButton
            {
                Source = "comment.png",
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Color.Transparent
            };

            var commentsLabel = new Label
            {
                Text = post.CommentCount.Count.ToString(),
                TextColor = Color.Black
            };

            likesDislikesLayout.Children.Add(likeButton);
            likesDislikesLayout.Children.Add(likesLabel);
            likesDislikesLayout.Children.Add(dislikeButton);
            likesDislikesLayout.Children.Add(dislikesLabel);
            likesDislikesLayout.Children.Add(commentsButton);
            likesDislikesLayout.Children.Add(commentsLabel);

            Editor comment = new Editor
            {
                Placeholder = "Geben Sie Ihren Kommentar ein...",
                PlaceholderColor = Color.Black,
                TextColor = Color.Black,
                HeightRequest = 50,
                BackgroundColor = Color.Transparent
            };

            var sendCommentLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10
            };

            var comsend = new ImageButton
            {
                Source = "send.png",
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Color.Transparent
            };

            sendCommentLayout.Children.Add(comment);
            sendCommentLayout.Children.Add(comsend);

            postLayout.Children.Add(userLayout);
            postLayout.Children.Add(postTextLabel);
            postLayout.Children.Add(likesDislikesLayout);
            postLayout.Children.Add(sendCommentLayout);

            return postLayout;
        }
        private async Task OpenUserProfile(User user)
        {
            // Erstellen Sie eine neue Instanz der UserProfile-Seite und übergeben Sie den Benutzer
            var userProfilePage = new UserProfile(user);

            // Navigieren Sie zur Benutzerprofilseite
            await Application.Current.MainPage.Navigation.PushAsync(userProfilePage);
        }
    }

   
}
