using App3.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App3
{
    public class DummyPost
    {
        public static async Task<List<Posting>> CreateDummyPosts()
        {
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler(); //Handler for certificates
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/"); //Client address
                client.DefaultRequestHeaders.Accept.Clear(); //Clear all Headers beforehand
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Post"); //API CALL
                //Check if the call responded with code 200 meaning OK
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync(); //Response content as String formatting
                    List<Posting> responseData = JsonConvert.DeserializeObject<List<Posting>>(jsonResponse); //Deseralize the content
                    var posts = new List<Posting>(); //Create new list for all posts
                    foreach (var post in responseData)
                    {
                        var newPost = new Posting //Create the post information in the Posting class
                        {
                            Id = post.Id,
                            Avatar = "placeholder_avatar.png",
                            Name = post.Name,
                            UserId = post.UserId,
                            Username = post.Username,
                            Text = post.Text,
                            UpVotes = post.UpVotes,
                            DownVotes = post.DownVotes,
                            CommentCount = new List<int> { 0 },
                            Date = post.Date
                        };
                        posts.Add(newPost); //Add the created post to the list
                    }
                    return posts;
                }
            }
            return null;
        }
        
    }
}
