using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3
{
    public class Logout : ContentPage
    {
        public Logout()
        {
            var logoutButton = new Button
            {
                Text = "Logout",
                Command = new Command(OnLogoutButtonClicked)
            };

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = { logoutButton }
            };
        }

        private void OnLogoutButtonClicked()
        {
            Application.Current.MainPage = new LoginPage();
            Settings.IsLoggedIn = false;
        }
    }
}
