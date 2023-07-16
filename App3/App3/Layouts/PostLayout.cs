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
        // Method to create a stack layout for a post
        {

            var postLayout = new StackLayout  
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Padding = new Thickness(10),
                BackgroundColor = Color.White,
            };

            var userLayout = new StackLayout  // contains Image, Username, Name and timestamp
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10
            };

            var avatarImage = new ImageButton // setting the avatar from the User Id
            {
                Source = post.Avatar,
                WidthRequest = 40,
                HeightRequest = 40,
                Aspect = Aspect.AspectFill
            };
            avatarImage.Clicked += async (sender, e) =>
            {
                await OpenUserProfile(post.UserId); // Button to get to the User-Profile, by following the UserID
            };
            var userInfoLayout = new StackLayout  // setting Info Layout, containing Name, Username and Timestamp
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
                Text = post.Date.ToString("d MMM"), // change Date to String, so it can been showen
                HorizontalOptions = LayoutOptions.EndAndExpand,
                TextColor = Color.Black
            };
            // Adding everithing to the UserInfoLayout
            userInfoLayout.Children.Add(nameLabel);
            usernameTimestampLayout.Children.Add(usernameLabel);
            usernameTimestampLayout.Children.Add(timestampLabel);
            userInfoLayout.Children.Add(usernameTimestampLayout);
            // Adding Picture and UserInfo to the Userlayout 
            userLayout.Children.Add(avatarImage);
            userLayout.Children.Add(userInfoLayout);

            var postTextLabel = new Label // set Post-Text 
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
            // checking if the User Íd is in the up-vote Array, to set the picture for the Like button 
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
            // see likebutton
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
            // structure when the likebutton is pushed
            likeButton.Clicked += async (s, e) =>
            {
                //checking if the UserId is in the upvote Array 
                if (post.UpVotes.Contains(currentUserId))
                {
                    // when the button was already pushed it needs to be removed and the picture of the button needs to be changed
                    post.UpVotes.Remove(currentUserId);
                    UpvotePost(post.Id, LoginPage.currentUser);
                    likeButton.Source = "thumb_up.png";
                }
                else
                {
                    // when the button wasen´t alredy pushed, the userId needs to set in the array and the buttonimage needs to be changed
                    post.UpVotes.Add(currentUserId);
                    UpvotePost(post.Id, LoginPage.currentUser);
                    likeButton.Source = "like_pushed.png";
                    // checking if the dislikebutton was already pushed, so it needs to be reseted 
                    if (post.DownVotes.Contains(currentUserId))
                    {
                        post.DownVotes.Remove(currentUserId);
                        dislikeButton.Source = "thumb_down.png";
                    }
                }
                // refresh Votecount
                likesLabel.Text = post.UpVotes.Count.ToString();
                dislikesLabel.Text = post.DownVotes.Count.ToString();
            };

            dislikeButton.Clicked += (s, e) =>
            { // see likeButton
                if (post.DownVotes.Contains(currentUserId))
                {
                    post.DownVotes.Remove(currentUserId);
                    DownvotePost(post.Id, LoginPage.currentUser);
                    dislikeButton.Source = "thumb_down.png";
                }
                else
                {
                    post.DownVotes.Add(currentUserId);
                    DownvotePost(post.Id, LoginPage.currentUser);
                    dislikeButton.Source = "dislike_pushed.png";

                    if (post.UpVotes.Contains(currentUserId))
                    {
                        post.UpVotes.Remove(currentUserId);
                        likeButton.Source = "thumb_up.png";
                    }
                }

                likesLabel.Text = post.UpVotes.Count.ToString();
                dislikesLabel.Text = post.DownVotes.Count.ToString();
            };
            // Button to get the comments of a post 
            var commentsButton = new ImageButton
            {
                Source = "comment.png",
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Color.Transparent
            };
            commentsButton.Clicked += async (sender, e) =>
            {
                await DisplayComments(post);
            };
            // counter for the posts 
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
            // Entry to for comments
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
            // Button to send a comment
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
                // saving the comment by UserId postId and commenttext
                CommentAdd new_comment = new CommentAdd
                {
                    UserId = LoginPage.currentUser.Id,
                    ParentPostId = post.Id,
                    Text = comment.Text,
                };
                
                // save the comment 
                SaveComment(new_comment);
                //resent the entry 
                comment.Text = string.Empty;
            }
            sendCommentLayout.Children.Add(comment);
            sendCommentLayout.Children.Add(comsend);

            postLayout.Children.Add(userLayout);
            postLayout.Children.Add(postTextLabel);
            postLayout.Children.Add(likesDislikesLayout);
            postLayout.Children.Add(sendCommentLayout);

            // giving back the hole postLayout 
            return postLayout;
        }

        private static async Task OpenUserProfile(string userID)
        {
            User clickedUser = await getUserData(userID);
            // Create a new instance of the UserProfile page and pass the user
            var userProfilePage = new UserProfile(clickedUser);

            // Navigate to the profilepage of the user
            await Application.Current.MainPage.Navigation.PushAsync(userProfilePage);
        }

        private static async Task<User> getUserData(string userID)
        {
            var token = LoginPage.token; //JWT Token for Authentication
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler(); //Handler for certificates
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/"); //Client address
                client.DefaultRequestHeaders.Accept.Clear(); //Clear all Headers beforehand
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token); //Add Authentication
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/User/" + userID); //API CALL
                //Check if call responded with code 200 meaning OK
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync(); //Response content as String formatting
                    JObject responseData = JsonConvert.DeserializeObject<JObject>(jsonResponse); //Deseralize the content

                    var clickedUser = new User //Creating the user information in User class
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
                    return clickedUser;
                }
                return null;
            }
        }

        public static async void UpvotePost(string postID, User user)
        {
            var token = LoginPage.token; //JWT Token for Authentication
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler(); //Handler for certificates
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/"); //Client address
                client.DefaultRequestHeaders.Accept.Clear(); //Clear all Headers beforehand
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token); //Add Authentication
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = JsonConvert.SerializeObject(user); //Seralize the object to send
                HttpResponseMessage response = await client.PostAsync("api/Post/" + postID + "/upvote", new StringContent(jsonPayload, Encoding.UTF8, "application/json")); //API CALL with content
            }
        }

        public static async void DownvotePost(string postID, User user)
        {
            var token = LoginPage.token; //JWT Token for Authentication
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler(); //Handler for certificates
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/"); //Client address
                client.DefaultRequestHeaders.Accept.Clear(); //Clear all Headers beforehand
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token); //Add Authentication
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = JsonConvert.SerializeObject(user); //Seralize the object to send
                HttpResponseMessage response = await client.PostAsync("api/Post/" + postID + "/downvote", new StringContent(jsonPayload, Encoding.UTF8, "application/json")); //API CALL with content
            }
        }
        public static async void SaveComment(CommentAdd comment)
        {
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler(); //Handler for certificates
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");  //Client address
                client.DefaultRequestHeaders.Accept.Clear();  //Clear all Headers beforehand
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = JsonConvert.SerializeObject(comment); //Seralize the object to send
                HttpResponseMessage response = await client.PostAsync("api/Comment/", new StringContent(jsonPayload, Encoding.UTF8, "application/json")); //API CALL with content
            }
        }
        private static async Task DisplayComments(Posting post)
        {
            // Navigate to the comments page, passing the post as a parameter
            await Application.Current.MainPage.Navigation.PushAsync(new CommentsPage(post));
        }
    }
   
}
