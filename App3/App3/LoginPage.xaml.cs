using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using App3.Services;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;

namespace App3
{
    public partial class LoginPage : ContentPage
    {
        bool isLoginValid = false;
        public static User currentUser;
        public static string userID;
        public static string token;
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            User user = new User
            {
                Email = usernameEntry.Text,
                Password = passwordEntry.Text
            };

            userID = await LoginUser(user);



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
        private async Task<string> LoginUser(User user)
        {
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = JsonConvert.SerializeObject(user);
                HttpResponseMessage response = await client.PostAsync("api/Authentication/Login", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject responseData = JsonConvert.DeserializeObject<JObject>(jsonResponse);
                    string userId = responseData["id"]?.ToString();
                    token = responseData["token"]?.ToString();

                    Console.WriteLine(responseData);

                    currentUser = new User
                    {
                        Id = (string)responseData["id"]?.ToString(),
                        Avatar = "placeholder_avatar.png",
                        Role = (Role)Enum.Parse(typeof(Role), responseData["role"]?.ToString()),
                        Username = (string)responseData["username"]?.ToString(),
                        Name = (string)responseData["name"]?.ToString(),
                        Email = (string)responseData["email"]?.ToString(),
                        Gender = responseData["gender"] != null ? (Gender?)Enum.Parse(typeof(Gender), responseData["gender"].ToString()) : null,
                        BirthDate = (string)responseData["birthDate"]?.ToString(),
                        Followers = JsonConvert.DeserializeObject<List<string>>(responseData["followers"]?.ToString()),
                        Following = JsonConvert.DeserializeObject<List<string>>(responseData["following"]?.ToString()),
                        CreatedAt = DateTime.Parse(responseData["createdAt"]?.ToString()),
                        Bio = (string)responseData["bio"]?.ToString(),
                        Interests = JsonConvert.DeserializeObject<List<string>>(responseData["interests"]?.ToString()),
                        Locked = (bool)responseData["locked"]
                    };
                    //Console.WriteLine(responseData);
                    isLoginValid = true;
                    return userId;
                }
                isLoginValid = false;
                return null;
            }
        }

    }
}
