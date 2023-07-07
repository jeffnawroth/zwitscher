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
                    List<Posting> responseData = JsonConvert.DeserializeObject<List<Posting>>(jsonResponse);
                    var posts = new List<Posting>();
                    Console.WriteLine(jsonResponse);
                    foreach (var post in responseData)
                    {
                        int upVotesCount = post.UpVotes?.Count(vote => vote != null && vote is string) ?? 0;
                        int downVotesCount = post.DownVotes?.Count(vote => vote != null && vote is string) ?? 0;

                        var newPost = new Posting
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
                        posts.Add(newPost);
                    }
                    return posts;
                }
            }
            return null;
        }
        
    }
}
