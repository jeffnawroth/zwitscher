using System;
using System.Collections.Generic;
using System.Text;
using App3.Services;


namespace App3.Services
{
    public class PostView
    {
        private Post post;

        public PostView(Post post)
        {
            this.post = post;
        }

        public void Display()
        {
            Console.WriteLine($"{post.UserName}");
            Console.WriteLine($"User Avatar: {post.UserAvatar}");
            Console.WriteLine($"Thumbs Up User IDs: {string.Join(", ", post.ThumbsUpUserIds)}");
            Console.WriteLine($"Thumbs Down User IDs: {string.Join(", ", post.ThumbsDownUserIds)}");
            Console.WriteLine($"Timestamp: {post.Timestamp}");
            Console.WriteLine();
        }
    }
}
