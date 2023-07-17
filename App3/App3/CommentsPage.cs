using App3.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using Xamarin.Forms;
using App3.Layouts;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App3
{
    //NOT WORKING 
    public class CommentsPage : ContentPage
    {
        public CommentsPage(Posting post)
        {
            Title = "Comments";

            InitializePageAsync(post);
            // Create a layout to display the comments
            
        }
        public static async Task<List<string>> GetIDsFromPosts()
        {
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Post");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    List<JObject> responseData = JsonConvert.DeserializeObject<List<JObject>>(jsonResponse);
                    var PostIDs = new List<string>();
                    //Console.WriteLine(jsonResponse);
                    foreach (var post in responseData)
                    {
                        var id = (string)post["id"]?.ToString();
                        PostIDs.Add(id);
                    }
                    Console.WriteLine(PostIDs.Count);
                    return PostIDs;
                }
            }
            return null;
        }
        private async Task<List<Posting>> GetCommentsForPost(List<string> IDs)
        {
            //var token = LoginPage.token;
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                foreach (var postId in IDs)
                {
                    //string jsonPayload = JsonConvert.SerializeObject(userID);
                    HttpResponseMessage response = await client.GetAsync("api/Comment/" + postId);
                    Console.WriteLine(response.StatusCode);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        List<Posting> responseData = JsonConvert.DeserializeObject<List<Posting>>(jsonResponse);
                        var comments = new List<Posting>();
                        //Console.WriteLine(jsonResponse);
                        foreach (var comment in responseData)
                        {
                            var newComment = new Posting
                            {
                                Id = comment.Id,
                                Avatar = "placeholder_avatar.png",
                                Name = comment.Name,
                                UserId = comment.UserId,
                                Username = comment.Username,
                                Text = comment.Text,
                                UpVotes = comment.UpVotes,
                                DownVotes = comment.DownVotes,
                                CommentCount = new List<int> { 0 },
                                Date = comment.Date
                            };
                            comments.Add(newComment);
                        }
                        return comments;
                    }
                }
            }
            return new List<Posting> { };
        }

        private async void InitializePageAsync(Posting post)
        {
            var commentsLayout = new StackLayout();

            var postIDS = await GetIDsFromPosts();
            // Retrieve the comments for the post
            var comments = await GetCommentsForPost(postIDS);
            
            // Create UI elements for each comment and add them to the commentsLayout
            foreach (var comment in comments)
            {
                /*
                var commentLabel = new Label
                {
                    Text = comment.Text
                };
                commentsLayout.Children.Add(commentLabel);
                */
            }
            
            // Add the commentsLayout to the Content of the CommentsPage
            Content = commentsLayout;

        }
    }
}