using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App3.Services;

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
            int characterCount = e.NewTextValue.Length;
            characterCoun.Text = characterCount.ToString() + " Zeichen";
        }
        private async void OnPostButtonClicked(object sender, EventArgs e)
        {
            string postContent = postingText.Text;

            // Hier können Sie die Logik zum Verarbeiten des Posts implementieren, z. B. das Speichern in der Datenbank oder das Senden an einen Server

            await DisplayAlert("Erfolgreich gepostet", "Der Beitrag wurde erfolgreich gepostet.", "OK");

            // Zurück zur vorherigen Seite navigieren oder eine andere Aktion ausführen
            await Navigation.PopAsync();
        }

    }

}

