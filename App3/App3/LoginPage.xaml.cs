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
            // Create a new User object with the entered email and password
            User user = new User
            {
                Email = usernameEntry.Text,
                Password = passwordEntry.Text
            };
            // Call the LoginUser method to authenticate the user and get the user ID
            userID = await LoginUser(user);


            // Check if the login is valid
            if (isLoginValid)
            {
                Settings.IsLoggedIn = true; // Set the IsLoggedIn setting to true
                User currentUser = new User(); // Create a new instance of the User class
                Application.Current.MainPage = new AppShell(currentUser); // Navigate to the AppShell page passing the current user
            }
            else
            {
                // Display an alert for invalid login credentials
                await DisplayAlert("Fehler", "Ungültige Anmeldeinformationen", "OK");
            }
        }

        private async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            // Navigate to the Registration page
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
                        Gender = responseData["gender"] != null && Enum.TryParse(responseData["gender"].ToString(), out Gender gender) ? (Gender?)gender : Gender.NULL,
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
