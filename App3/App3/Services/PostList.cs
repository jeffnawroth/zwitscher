using System;
using System.Collections.Generic;
using System.Text;
using App3.Services;

namespace App3.Services
{
    public class PostList
    {
        private List<Post> posts;

        public PostList(List<Post> posts)
        {
            this.posts = posts;
        }

        public void Display()
        {
            foreach (var post in posts)
            {
                PostView postView = new PostView(post);
                postView.Display();
            }
        }
    }
}
