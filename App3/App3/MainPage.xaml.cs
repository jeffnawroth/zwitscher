using App3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace App3
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
/*{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            if (username == "admin" && password == "password")
            {
                // open the app
                DisplayAlert("Erfolgreich", "Anmeldung erfolgreich!", "OK");
            }
            else
            {
                DisplayAlert("Fehler", "Ungültige Anmeldeinformationen!", "OK");
            }
        }
    }
}
*/
