using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

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
                await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Fehler", "Ungültige Anmeldeinformationen", "OK");
            }
        }
    }
}
