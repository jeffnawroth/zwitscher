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
using Newtonsoft.Json.Linq;

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
            // Retrieve input values from the form
            string username = usernameEntry.Text;
            string name = nameEntry.Text;
            string email = emailEntry.Text;
            string password = passwordEntry.Text;
            string confirmPassword = confirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                // check if every entry is set 
                await DisplayAlert("Fehler", "Bitte füllen Sie alle Felder aus.", "OK");
                return;
            }

            User user = new User //create temporary User instance
            {
                Username = username,
                Name = name,
                Email = email,
                Password = password,
            };

            bool isRegistrationSuccessful = await RegisterUser(user); //Check if information is already in Database and if they are correct to register
            //if information correct
            if (isRegistrationSuccessful)
            {
                Settings.IsLoggedIn = true;
                Application.Current.MainPage = new AppShell(user);
            }
            else //When not display message
            {
                await DisplayAlert("Fehler", "Die Registrierung ist fehlgeschlagen.", "OK");
            }
        } 
        private async Task<bool> RegisterUser(User user)
        {
            HttpClientHandler insecureHandler = GetInsecureHandler(); //Handler for certificates
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/"); //Client address
                client.DefaultRequestHeaders.Accept.Clear(); //Clear all Headers beforehand
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = JsonConvert.SerializeObject(user); //Serialize the object to send
                HttpResponseMessage response = await client.PostAsync("api/Authentication/Register", new StringContent(jsonPayload, Encoding.UTF8, "application/json")); //API CALL with content
                //Check if call responded with code 200 meaning OK
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync(); //Response content as String formatting
                    JObject responseData = JsonConvert.DeserializeObject<JObject>(jsonResponse); //Deseralize the content
                    LoginPage.token = responseData["token"]?.ToString(); //save the recieved JWT token

                    LoginPage.currentUser = new User //save the recieved information for the User
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
                    return true;
                }
                return false;
            }
        }

            public static HttpClientHandler GetInsecureHandler()
            {
                HttpClientHandler handler = new HttpClientHandler();
                //Custom CertificateHandler to accept the certificates from the localhost
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
