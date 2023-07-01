using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using App3.Services;

namespace App3
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;

            bool isLoginValid = await LoginService.ValidateCredentials(username, password);

            if (isLoginValid)
            {
                Settings.IsLoggedIn = true;
                User currentUser = new User();
                Application.Current.MainPage = new AppShell(currentUser);
            }
            else
            {
                await DisplayAlert("Fehler", "Ungültige Anmeldeinformationen", "OK");
            }
        }

        private async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new Registration()));
        }

    }
}
