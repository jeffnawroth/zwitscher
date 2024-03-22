using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace App3
{
    public class ChangePasswordPage : ContentPage
    {
        private Entry oldPasswordEntry;
        private Entry newPasswordEntry;
        private Entry confirmPasswordEntry;

        public ChangePasswordPage()  // Implement Layout 
        {
            Title = "Passwort ändern";
            

            newPasswordEntry = new Entry // Entry for the new Password
            {
                Placeholder = "Neues Passwort",
                IsPassword = true,
                PlaceholderColor = Color.Black,
                TextColor = Color.Black
            };

            confirmPasswordEntry = new Entry // Entry to confirm the Password
            {
                Placeholder = "Passwort bestätigen",
                IsPassword = true,
                PlaceholderColor = Color.Black,
                TextColor = Color.Black
            };

            var changeButton = new Button // Button to check and send at Backend 
            {
                Text = "Passwort ändern",
                TextColor = Color.Black,
                BackgroundColor = Color.LightGray
            };
            changeButton.Clicked += OnChangePasswordButtonClicked;

            Content = new StackLayout  // Layout fitting 
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    newPasswordEntry,
                    confirmPasswordEntry,
                    changeButton
                }
            };
        }

        private void OnChangePasswordButtonClicked(object sender, EventArgs e)  // Button Function 
        {
            // read in entrys 
            string newPassword = newPasswordEntry.Text;
            string confirmPassword = confirmPasswordEntry.Text;

            // check if entrys match, and change if they do 
            if (CheckNewPasswordsMatch(newPassword, confirmPassword))
            {
                // change Password 
                ChangePassword(newPassword);

                // reset entry 
                newPasswordEntry.Text = string.Empty;
                confirmPasswordEntry.Text = string.Empty;
            }
            else
            {
                // Error if the passwords don´t match 
                DisplayAlert("Fehler", "Neue Passwörter stimmen nicht überein.", "OK");
            }
        }
      

        private bool CheckNewPasswordsMatch(string newPassword, string confirmPassword)
        {
            // Password check 
            return newPassword == confirmPassword;
        }

        private async void ChangePassword(string newPassword)
        {
            var token = LoginPage.token; //JWT Token for Authentication
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler(); //Handler for certificates
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/"); //Client address
                client.DefaultRequestHeaders.Accept.Clear(); //Clear all Headers beforehand
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token); //Add Authentication
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = JsonConvert.SerializeObject(newPassword); //Seralize the object to send
                HttpResponseMessage response = await client.PutAsync("api/User/PasswordChange?new_password=" + newPassword, new StringContent(jsonPayload, Encoding.UTF8, "application/json")); //API CALL with 
                //Check if the call responded with code 200 meaning OK
                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Erfolg", "Das Password wurde erfolgreich geändert", "OK");
                }
                else
                {
                    DisplayAlert("Fehler", "Das Password wurde nicht geändert", "OK");
                }
            }
        }
    }
}
