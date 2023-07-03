using App3.Layouts;
using App3.Services;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace App3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
  

            var postsStackLayout = new StackLayout();

            var dummyPosts = DummyPost.CreateDummyPosts();

            foreach (var post in dummyPosts)
            {
                var postLayout = PostLayout.CreatePostLayout(post, currentUserId);
                postsStackLayout.Children.Add(postLayout);
            }

            var scrollView = new ScrollView
            {
                Content = postsStackLayout
            };

            Content = scrollView;
        }

        private int currentUserId = 1;
    }
}
