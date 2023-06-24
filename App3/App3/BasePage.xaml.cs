using System;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;

namespace App3
{
    public partial class BasePage : ContentPage
    {
        public BasePage()
        {
            // Erstellen Sie den Button
            var button = new Button
            {
                Text = "Post",
                BackgroundColor = Color.Transparent,
                TextColor = Color.White,
                CornerRadius = 30,
                WidthRequest = 60,
                HeightRequest = 60,
                FontSize = 24,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(20)
            };

            // Fügen Sie dem Button das entsprechende Symbol hinzu
            button.ImageSource = "sleep_icon.png";

            // Definieren Sie die Klickereignisbehandlung
            button.Clicked += OnPostButtonClicked;

            // Fügen Sie den Button zur Content-Seite hinzu
            Content = new StackLayout
            {
                Children = { button }
            };
        }

        private async void OnPostButtonClicked(object sender, EventArgs e)
        {
            // Öffnen Sie die Posting-Seite als Popup
            var postingPage = new PostingPopUp();
            await PopupNavigation.Instance.PushAsync(postingPage);
        }
    }
}

