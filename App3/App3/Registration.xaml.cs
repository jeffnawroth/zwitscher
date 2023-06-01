using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App3.Services;


namespace App3
{
    public partial class Registration : ContentPage
    {
        public Registration()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string vorname = vornameEntry.Text;
            string nachname = nachnameEntry.Text;
            string email = emailEntry.Text;
            string password = passwordEntry.Text;
            string confirmPassword = confirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(vorname) || string.IsNullOrWhiteSpace(nachname) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                 await DisplayAlert("Fehler", "Bitte füllen Sie alle Felder aus.", "OK");
                return;
            }

            if (Services.DummyBackend.IsUserRegistered(email))
            {
                await DisplayAlert("Fehler", "Die E-Mail-Adresse ist bereits registriert.", "OK");
                return;
            }

            bool isRegistrationSuccessful = Services.DummyBackend.SaveRegistrationData(username, vorname, nachname, email, password);
            if (isRegistrationSuccessful)
            {
                Settings.IsLoggedIn = true;
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await DisplayAlert("Fehler", "Die Registrierung ist fehlgeschlagen.", "OK");
            }
        }

    }
}
