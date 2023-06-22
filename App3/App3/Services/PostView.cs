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
            Console.WriteLine($"Post ID: {post.PostId}");
            Console.WriteLine($"User ID: {post.UserId}");
            Console.WriteLine($"Content: {post.Content}");
            Console.WriteLine($"Timestamp: {post.Timestamp}");
            Console.WriteLine();
        }
    }
}
