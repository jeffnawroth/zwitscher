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
    public partial class PostingPage : ContentPage
    {
        public PostingPage()
        {
            InitializeComponent();
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // Calculate the character count of the entered text
            int characterCount = e.NewTextValue.Length;
            characterCou.Text = characterCount.ToString() + " Zeichen";
        }
        private async void OnPostButtonClicked(object sender, EventArgs e)
        {
            CreatePost post = new CreatePost // Create a new post object
            {
                UserId = LoginPage.userID,
                Text = postingText.Text
            };
            // Attempt to create the post
            bool isPostOkay = await CreatePost(post);
            if(isPostOkay) // Display a success alert if the post was created successfully
            {
                await DisplayAlert("Erfolgreich gepostet", "Der Beitrag wurde erfolgreich gepostet.", "OK");
                postingText.Text = string.Empty;
            }
            else // Display an error alert if the post creation failed
            {
                await DisplayAlert("Fehlgeschlagen", "Der Beitrag wurde nicht gepostet", "Ok");
            }
            await Navigation.PopAsync();  // Navigate back to the previous page
        }

        private async Task<bool> CreatePost(CreatePost post)
        {
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonPayload = JsonConvert.SerializeObject(post);
                // Send a POST request to create the post
                HttpResponseMessage response = await client.PostAsync("api/Post", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    HttpContent returnValues = response.Content;
                    Console.WriteLine(returnValues);
                    return true;  // Post creation successful
                }
                return false; // psot creation failed
            }
        }

    }

}

