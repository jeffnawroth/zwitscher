using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
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
            // set Layout 
            InitializeComponent();
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool deleteOkay = await Delete(); // start account-delate
            if(deleteOkay)
            {
                await DisplayAlert("Erfolgreich gelöscht", "Dein Konto wurde erfolgreich gelöscht.", "OK");
                Application.Current.MainPage = new LoginPage();  // go back to LoginPage
                Settings.IsLoggedIn = false;  // reset the login-Status
            }
            else
            {
                // error warning if it delate doesn´t work
                await DisplayAlert("Löschung fehlgeschlagen", "Dein Konto wurde nicht gelöscht.", "OK");
            }

        }

        public async Task<bool> Delete()
        {
            string token = LoginPage.token;
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response = await client.DeleteAsync("api/User/" + LoginPage.userID);

                if(response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
