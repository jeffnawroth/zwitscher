using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using App3.Services;

namespace App3.Layouts
{
    public class EditProfilePopup : PopupPage
    {
        public EditProfilePopup(User user)
        {
            var mainLayout = new StackLayout
            {
                Padding = new Thickness(20),
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var nameLabel = new Label
            {
                Text = "Name:",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            var nameEntry = new Entry
            {
                Text = user.Name,
                Placeholder = "Enter your name",
                FontSize = 14,
                TextColor = Color.Black
            };

            var birthDateLabel = new Label
            {
                Text = "Geburtsdatum:",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            var birthDatePicker = new DatePicker
            {
                Date = user.BirthDate,
                Format = "dd.MM.yyyy",
                TextColor = Color.Black
            };

            var genderLabel = new Label
            {
                Text = "Geschlecht:",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            var genderPicker = new Picker
            {
                Title = "Geschlecht auswählen",
                TextColor = Color.Black
            };
            genderPicker.Items.Add("Männlich");
            genderPicker.Items.Add("Weiblich");
            genderPicker.Items.Add("Divers");
            genderPicker.SelectedItem = user.Gender == Gender.Male ? "Männlich" : user.Gender == Gender.Female ? "Weiblich" : "Divers";

            var bioLabel = new Label
            {
                Text = "Bio:",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            var bioEditor = new Editor
            {
                Text = user.Bio,
                FontSize = 14,
                TextColor = Color.Black,
                HeightRequest = 100
            };

            var interestsLabel = new Label
            {
                Text = "Interessen:",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };

            var interestsEditor = new Editor
            {
                Text = string.Join(", ", user.Interests),
                FontSize = 14,
                TextColor = Color.Black,
                HeightRequest = 100
            };

            var saveButton = new Button
            {
                Text = "Speichern",
                FontSize = 16,
                BackgroundColor = Color.Green,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            

            mainLayout.Children.Add(nameLabel);
            mainLayout.Children.Add(nameEntry);
            mainLayout.Children.Add(birthDateLabel);
            mainLayout.Children.Add(birthDatePicker);
            mainLayout.Children.Add(genderLabel);
            mainLayout.Children.Add(genderPicker);
            mainLayout.Children.Add(bioLabel);
            mainLayout.Children.Add(bioEditor);
            mainLayout.Children.Add(interestsLabel);
            mainLayout.Children.Add(interestsEditor);
            mainLayout.Children.Add(saveButton);

            Content = mainLayout;
        }

        
    }
}
