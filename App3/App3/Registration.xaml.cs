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
            string name = nameEntry.Text;
            string email = emailEntry.Text;
            string password = passwordEntry.Text;
            string confirmPassword = confirmPasswordEntry.Text;
            int id = GenerateUserID();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Fehler", "Bitte füllen Sie alle Felder aus.", "OK");
                return;
            }

            if (Database.IsUserRegistered(email))
            {
                await DisplayAlert("Fehler", "Die E-Mail-Adresse ist bereits registriert.", "OK");
                return;
            }

            User user = new User
            {
                Id = id,
                Username = username,
                Name = name,
                Email = email,
                Password = password,
                // Setzen Sie weitere Eigenschaften des Benutzers...
            };

            bool isRegistrationSuccessful = Database.SaveRegistrationData(user);
            if (isRegistrationSuccessful)
            {
                Settings.IsLoggedIn = true;
                Application.Current.MainPage = new AppShell(user);
            }
            else
            {
                await DisplayAlert("Fehler", "Die Registrierung ist fehlgeschlagen.", "OK");
            }
        }

        private int GenerateUserID()
        {
            return new Random().Next(1000, 9999);
        }
    }
}
