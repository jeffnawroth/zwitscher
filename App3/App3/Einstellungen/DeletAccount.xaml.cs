using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3
{
    public partial class DeletAccount : ContentPage
    {
        public DeletAccount()
        {
            InitializeComponent();
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            // Code to deleat the account 
            await DisplayAlert("Erfolgreich gelöscht", "Dein Konto wurde erfolgreich gelöscht.", "OK");
            Application.Current.MainPage = new LoginPage();
            Settings.IsLoggedIn = false;
        }
    }
}
