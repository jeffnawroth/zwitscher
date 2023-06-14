using App3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using System.Collections.ObjectModel;

namespace App3
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> Posts { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Posts = new ObservableCollection<string>();
            postListView.ItemsSource = Posts;
        }

        public void AddPost(string postText)
        {
            Posts.Add(postText);
        }
    }
}
