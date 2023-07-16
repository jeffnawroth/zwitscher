using App3.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using Xamarin.Forms;
using System.Text;

namespace App3
{
    public class ChangeEMail: ContentPage
    {
        private Entry oldEmailEntry;
        private Entry newEmailEntry;
        private Entry confirmEmailEntry;

        public ChangeEMail() //Starting the Layout
        {
            Title = "E-Mail ändern";

            oldEmailEntry = new Entry //Input for the old Email-Adress
            {
                Placeholder = "Alte E-Mail",
                PlaceholderColor = Color.Black,
                TextColor = Color.Black,
            };

            newEmailEntry = new Entry //Input for the new Email-Adress
            {
                Placeholder = "Neue E-Mail",
                PlaceholderColor = Color.Black,
                TextColor = Color.Black,
            };

            confirmEmailEntry = new Entry //Input to confirm the new Email-Adress
            {
                Placeholder = "E-Mail bestätigen",
                PlaceholderColor = Color.Black,
                TextColor = Color.Black,
            };

            var changeButton = new Button //Button to check and send at Backend 
            {
                Text = "E-Mail ändern"
            };
            changeButton.Clicked += OnChangeEmailButtonClicked;

            Content = new StackLayout  // Layout fitting 
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    oldEmailEntry,
                    newEmailEntry,
                    confirmEmailEntry,
                    changeButton
                }
            };
        }

        private void OnChangeEmailButtonClicked(object sender, EventArgs e)
        {
            string oldEmail = oldEmailEntry.Text;
            string newEmail = newEmailEntry.Text;
            string confirmEmail = confirmEmailEntry.Text;
            // read in the tiped Information
            // check and change the Email, if valued
            if (CheckOldEmail(oldEmail) && CheckNewEmailsMatch(newEmail, confirmEmail))
            {
                
                ChangeEmail(newEmail);

                // Reset Entrys
                oldEmailEntry.Text = string.Empty;
                newEmailEntry.Text = string.Empty;
                confirmEmailEntry.Text = string.Empty;
            }
            else
            {
                DisplayAlert("Fehler", "Falsche E-Mail-Adresse oder neue E-Mail-Adressen stimmen nicht überein.", "OK");
            }
        }


        private bool CheckOldEmail(string email)
        {
            // Check if the entered Email is the old Email; return true if right and false if not 
            if(email == LoginPage.currentUser.Email)
            {
                return true;
            }
            return false;
        }

        private bool CheckNewEmailsMatch(string newEmail, string confirmEmail)
        {
            // Check if new Email and confirmed Email are equal
            return newEmail == confirmEmail;
        }

        private async void ChangeEmail(string newEmail)
        {
            
            var token = LoginPage.token;
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = JsonConvert.SerializeObject(newEmail);
                HttpResponseMessage response = await client.PutAsync("api/User/EmailChange?new_email=" + newEmail, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
                Console.WriteLine(response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Erfolg", "Die Email wurde erfolgreich geändert", "OK");
                }
                else
                {
                    DisplayAlert("Fehler", "Die Email wurde nicht geändert", "OK");
                }
            }
        }
    }
}
