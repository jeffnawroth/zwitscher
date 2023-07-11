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
            InitializePageAsync();
        }
        private async void InitializePageAsync()
        { 
            var postsStackLayout = new StackLayout();

            var dummyPosts = await DummyPost.CreateDummyPosts(); //ist nicht dummy sondern richtige werte

            foreach (var post in dummyPosts)
            {
                var postLayout = PostLayout.CreatePostLayout(post, LoginPage.userID);
                postsStackLayout.Children.Add(postLayout);
            }

            var scrollView = new ScrollView
            {
                Content = postsStackLayout
            };

            Content = scrollView;
        }
    }
}
