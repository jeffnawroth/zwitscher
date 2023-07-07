using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App3.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;

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
            //int id = GenerateUserID();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Fehler", "Bitte füllen Sie alle Felder aus.", "OK");
                return;
            }

            User user = new User
            {
                //Id = id,
                Username = username,
                Name = name,
                Email = email,
                Password = password,
                // Setzen Sie weitere Eigenschaften des Benutzers...
            };

            bool isRegistrationSuccessful = await RegisterUser(user);
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
            private async Task<bool> RegisterUser(User user)
            {
                HttpClientHandler insecureHandler = GetInsecureHandler();
                using (var client = new HttpClient(insecureHandler))
                {
                    client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonPayload = JsonConvert.SerializeObject(user);
                    HttpResponseMessage response = await client.PostAsync("api/Authentication/Register", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        HttpContent returnValues = response.Content;
                        Console.WriteLine(returnValues);
                        return true;
                    }
                    return false;
                }
            }

            public static HttpClientHandler GetInsecureHandler()
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (cert.Issuer.Equals("CN=localhost"))
                        return true;
                    return errors == System.Net.Security.SslPolicyErrors.None;
                };
                return handler;
            }
    }
}
