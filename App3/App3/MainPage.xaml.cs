using App3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var logoutButton = new Button
            {
                Text = "Logout",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            logoutButton.Clicked += OnLogoutButtonClicked;

            Content = new StackLayout
            {
                Children = { logoutButton }
            };


        }
        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
            Settings.IsLoggedIn = false;
        }
        }
}