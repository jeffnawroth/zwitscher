using Xamarin.Forms;
using System.Collections.Generic;
using App3.Services;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System;
using Newtonsoft.Json.Linq;

namespace App3.Layouts
{
    public class PostLayout
    {
        public static StackLayout CreatePostLayout(Posting post, string currentUserId)
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
                await OpenUserProfile(post.UserId);
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
                Text = post.Date.ToString("d MMM"),
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
                Text = post.Text,
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
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Color.Transparent
            };

            if (post.UpVotes.Contains(currentUserId))
            {
                likeButton.Source = "like_pushed.png";
            }
            else
            {
                likeButton.Source = "thumb_up.png";
            }


            var likesLabel = new Label
            {   
                Text = post.UpVotes.Count.ToString(),
                TextColor = Color.Black
            };

            var dislikeButton = new ImageButton
            {
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Color.Transparent
            };

            if (post.DownVotes.Contains(currentUserId))
            {
                dislikeButton.Source = "dislike_pushed.png";
            }
            else
            {
                dislikeButton.Source = "thumb_down.png";
            }

            var dislikesLabel = new Label
            {
                Text = post.DownVotes.Count.ToString(),
                TextColor = Color.Black
            };
            
            likeButton.Clicked += async (s, e) =>
            {
                if (post.UpVotes.Contains(currentUserId))
                {
                    post.UpVotes.Remove(currentUserId);
                    UpvotePost(post.Id, LoginPage.currentUser);
                    //user.LikedPosts.Remove(post.PostId);
                    likeButton.Source = "thumb_up.png";
                }
                else
                {
                    post.UpVotes.Add(currentUserId);
                    UpvotePost(post.Id, LoginPage.currentUser);
                    //user.DislikedPosts.Add(post.PostId);
                    likeButton.Source = "like_pushed.png";

                    if (post.DownVotes.Contains(currentUserId))
                    {
                        post.DownVotes.Remove(currentUserId);
                        //user.DislikedPosts.Remove(post.PostId);
                        dislikeButton.Source = "thumb_down.png";
                    }
                }

                likesLabel.Text = post.UpVotes.Count.ToString();
                dislikesLabel.Text = post.DownVotes.Count.ToString();
            };

            dislikeButton.Clicked += (s, e) =>
            {
                if (post.DownVotes.Contains(currentUserId))
                {
                    post.DownVotes.Remove(currentUserId);
                    DownvotePost(post.Id, LoginPage.currentUser);
                    //user.DislikedPosts.Remove(post.PostId);
                    dislikeButton.Source = "thumb_down.png";
                }
                else
                {
                    post.DownVotes.Add(currentUserId);
                   // user.DislikedPosts.Add(post.PostId);
                    dislikeButton.Source = "dislike_pushed.png";

                    if (post.UpVotes.Contains(currentUserId))
                    {
                        post.UpVotes.Remove(currentUserId);
                        DownvotePost(post.Id, LoginPage.currentUser);
                        // user.LikedPosts.Remove(post.PostId);
                        likeButton.Source = "thumb_up.png";
                    }
                }

                likesLabel.Text = post.UpVotes.Count.ToString();
                dislikesLabel.Text = post.DownVotes.Count.ToString();
            };

            var commentsButton = new ImageButton
            {
                Source = "comment.png",
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Color.Transparent
            };
            /*commentsButton.Clicked += async (sender, e) =>
            {
                // Hier kannst du den Code einfügen, um die Kommentare zum Post anzuzeigen
                await DisplayComments(post);
            };

            async Task DisplayComments(Posting post)
            {
               
            }*/

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
            comsend.Clicked += OnSendCommentClicked;

            void OnSendCommentClicked(object sender, EventArgs e)
            {
                string commentText = comment.Text;
                // Führe die Aktionen aus, um den Kommentar zu speichern
                SaveComment(commentText);
                // Zurücksetzen des Kommentar-Editors
                comment.Text = string.Empty;
            }

            void SaveComment(string commentText)
            {
                // Hier kannst du den Code einfügen, um den Kommentar zu speichern
                // Verwende die commentText-Variable, um auf den eingegebenen Kommentartext zuzugreifen
                // Du kannst beispielsweise den Kommentar in einer Datenbank speichern oder eine API-Anfrage senden
            }
            sendCommentLayout.Children.Add(comment);
            sendCommentLayout.Children.Add(comsend);

            postLayout.Children.Add(userLayout);
            postLayout.Children.Add(postTextLabel);
            postLayout.Children.Add(likesDislikesLayout);
            postLayout.Children.Add(sendCommentLayout);

            return postLayout;
        }
        private static async Task OpenUserProfile(string userID)
        {
            User clickedUser = await getUserData(userID);
            // Erstellen Sie eine neue Instanz der UserProfile-Seite und übergeben Sie den Benutzer
            var userProfilePage = new UserProfile(clickedUser);

            // Navigieren Sie zur Benutzerprofilseite
            await Application.Current.MainPage.Navigation.PushAsync(userProfilePage);
        }

        private static async Task<User> getUserData(string userID)
        {
            var token = LoginPage.token;
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //string jsonPayload = JsonConvert.SerializeObject(userID);
                HttpResponseMessage response = await client.GetAsync("api/User/" + userID);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject responseData = JsonConvert.DeserializeObject<JObject>(jsonResponse);
                    string userId = responseData["id"]?.ToString();
                    token = responseData["token"]?.ToString();

                    Console.WriteLine(responseData);

                    var clickedUser = new User
                    {
                        Id = (string)responseData["id"]?.ToString(),
                        Avatar = "placeholder_avatar.png",
                        Role = (Role)Enum.Parse(typeof(Role), responseData["role"]?.ToString()),
                        Username = (string)responseData["username"]?.ToString(),
                        Name = (string)responseData["name"]?.ToString(),
                        Email = (string)responseData["email"]?.ToString(),
                        Gender = responseData["gender"] != null ? (Gender?)Enum.Parse(typeof(Gender), responseData["gender"].ToString()) : null,
                        BirthDate = (string)responseData["birthDate"]?.ToString(),
                        Followers = JsonConvert.DeserializeObject<List<string>>(responseData["followers"]?.ToString()),
                        Following = JsonConvert.DeserializeObject<List<string>>(responseData["following"]?.ToString()),
                        CreatedAt = DateTime.Parse(responseData["createdAt"]?.ToString()),
                        Bio = (string)responseData["bio"]?.ToString(),
                        Interests = JsonConvert.DeserializeObject<List<string>>(responseData["interests"]?.ToString()),
                        Locked = (bool)responseData["locked"]
                    };
                    //Console.WriteLine(responseData);
                    return clickedUser;
                }
                return null;
            }
        }

        public static async void UpvotePost(string postID, User user)
        {
            var token = LoginPage.token;
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = JsonConvert.SerializeObject(user);
                HttpResponseMessage response = await client.PostAsync("api/Post/" + postID + "/upvote", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
            }
        }

        public static async void DownvotePost(string postID, User user)
        {
            var token = LoginPage.token;
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = JsonConvert.SerializeObject(user);
                HttpResponseMessage response = await client.PostAsync("api/Post/" + postID + "/downvote", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
            }
        }

    }
   
}
