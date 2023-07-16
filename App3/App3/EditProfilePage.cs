using App3.Services;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace App3
{
    public class EditProfilePage : ContentPage
    {
        private User user;
        private readonly Entry nameEntry;
        private readonly DatePicker birthDateDatePicker;
        private readonly Picker genderPicker;
        private Editor bioEditor;

        public EditProfilePage(User user)
        {
            this.user = user;
            // Create the main layout for the page
            var mainLayout = new StackLayout
            {
                Spacing = 10,
                Padding = new Thickness(10),
                BackgroundColor = Color.White,
            };
            // Create the save button
            var saveButton = new ImageButton
            {
                Source = "save_profile.png",
                BackgroundColor = Color.LightGray,
                WidthRequest = 20,
                HeightRequest = 20,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            saveButton.Clicked += SaveButton_Clicked;
            mainLayout.Children.Add(saveButton);
            // create a avtarButton to change Image
            var avatarButton = new ImageButton
            {
                Source = user.Avatar,
                WidthRequest = 180,
                HeightRequest = 180,
                // Aspect = Aspect.AspectFill,
                BackgroundColor = Color.Transparent
            };
            avatarButton.Clicked += async (sender, e) =>
            {
                // Open the media picker to select a new image
                var pickResult = await MediaPicker.PickPhotoAsync();

                if (pickResult != null)
                {
                    // Update the user's avatar path with the selected image's full path
                    user.Avatar = pickResult.FullPath;

                    // Update the image source of the avatar button to display the selected image
                    avatarButton.Source = ImageSource.FromFile(pickResult.FullPath);
                }
            };
            mainLayout.Children.Add(avatarButton);
            // create name Entry 
            nameEntry = new Entry
            {
                Placeholder = "Name",
                Text = user.Name,
                TextColor = Color.Black,
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            mainLayout.Children.Add(nameEntry);
            // create username Label so it cant be changed 
            var usernameLabel = new Label
            {
                Text = "@" + user.Username,
                FontSize = 24,
                TextColor = Color.Black

            };

            // create followers and following Labels and Layout 
            var followersLabel = new Label
            {
                Text = $"{user.Followers.Count} Followers",
                TextColor = Color.Black,
                FontSize = 24
            };
            var followingLabel = new Label
            {
                Text = $"{user.Following.Count} Following",
                TextColor = Color.Black,
                FontSize = 24
            };
            var followersLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Children = { usernameLabel, followersLabel, followingLabel },
                HorizontalOptions = LayoutOptions.Center
            };
            mainLayout.Children.Add(followersLayout);
            var infoLayout = new FlexLayout
            {
                Direction = FlexDirection.Row,
                Wrap = FlexWrap.Wrap,
                JustifyContent = FlexJustify.SpaceBetween,
                AlignItems = FlexAlignItems.Center,
                AlignContent = FlexAlignContent.Center,
                Margin = new Thickness(0, 10)
            };
            // Create a date picker for the birth date
            birthDateDatePicker = new DatePicker
            {
                TextColor = Color.Black,
                Format = "dd.MM.yyyy",
                MaximumDate = DateTime.Today,
                Date = DateTime.Parse(user.BirthDate),
                FontSize = 20
            };
            var birthDateLayout = CreateViewWithIcon("geboren: ", birthDateDatePicker, "birthday.png");
            infoLayout.Children.Add(birthDateLayout);

            var joinDateLabel = new Label
            {
                Text = $"{user.CreatedAt.ToString("yyyy-MM-dd")}",
                TextColor = Color.Black,
                FontSize = 20
            };
            var joinDateLayout = CreateViewWithIcon("beigetreten: ", joinDateLabel, "startday.png");
            infoLayout.Children.Add(joinDateLayout);
            // create a picker to change gender
            genderPicker = new Picker
            {
                TextColor = Color.Black,
                FontSize = 20
            };
            genderPicker.Items.Add("Male");
            genderPicker.Items.Add("Female");
            genderPicker.Items.Add("Diverse");
            genderPicker.SelectedItem = user.Gender.ToString();
            var genderLayout = CreateViewWithIcon("Gender", genderPicker, GetGenderIconImage(user.Gender.Value));
            infoLayout.Children.Add(genderLayout);
            mainLayout.Children.Add(infoLayout);
            var interestsLabel = new Label
            {
                Text = "Interessen",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.Center
            };
            mainLayout.Children.Add(interestsLabel);
            // create Layout for interests 
            var interestsLayout = new FlexLayout
            {
                Wrap = FlexWrap.Wrap,
                JustifyContent = FlexJustify.SpaceBetween,
                AlignItems = FlexAlignItems.Start,
                AlignContent = FlexAlignContent.Start,
                Margin = new Thickness(0, 5)
            };
            // Create interest buttons for each interest
            foreach (var interest in user.Interests)
            {
                var interestButton = CreateInterestButton(interest);
                interestsLayout.Children.Add(interestButton);
            }
            // Create a button for adding a new interest
            var addInterestButton = new Button
            {
                Text = "+",
                TextColor = Color.Black,
                BackgroundColor = Color.LightGray,
                FontSize = 10,
                Padding = new Thickness(5),
                Margin = new Thickness(5),
                BorderColor = Color.Transparent
            };

            addInterestButton.Clicked += async (sender, e) =>
            {
                // Display a prompt dialog to enter a new interest
                string newInterest = await DisplayPromptAsync("Neues Interesse hinzufügen", "Geben Sie das neue Interesse ein", "Hinzufügen", "Abbrechen");

                if (!string.IsNullOrWhiteSpace(newInterest))
                {
                    // Create a new interest button with the entered interest text
                    var newInterestButton = CreateInterestButton(newInterest);

                    // Insert the new interest button before the last button (the add button)
                    interestsLayout.Children.Insert(interestsLayout.Children.Count - 1, newInterestButton);

                    // Add the new interest to the user's interests collection
                    user.Interests.Add(newInterest);
                }
            };


            interestsLayout.Children.Add(addInterestButton);

            Button CreateInterestButton(string interest)
            {
                // Create a new Button for the interest
                var interestButton = new Button
                {
                    Text = interest,
                    TextColor = Color.Black,
                    BackgroundColor = Color.LightGray,
                    FontSize = 24,
                    Padding = new Thickness(5),
                    Margin = new Thickness(5),
                    BorderColor = Color.Transparent
                };

                interestButton.Clicked += async (sender, e) =>
                {
                    // Display a confirmation alert dialog before deleting the interest
                    bool deleteConfirmed = await DisplayAlert("Interesse löschen", "Möchten Sie das Interesse wirklich löschen?", "Ja", "Nein");

                    if (deleteConfirmed)
                    {
                        // Remove the interest button from the interests layout
                        interestsLayout.Children.Remove(interestButton);

                        // Remove the interest from the user's interests collection
                        user.Interests.Remove(interest);
                    }
                };

                return interestButton;
            }

            mainLayout.Children.Add(interestsLayout);
            // create Bioeditor
            bioEditor = new Editor
            {
                Text = user.Bio,
                TextColor = Color.Black,
                FontSize = 14,
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            bioEditor.TextChanged += (sender, e) =>
            {
                var newBio = bioEditor.Text;
                user.Bio = newBio;
            };

            mainLayout.Children.Add(bioEditor);


            Content = mainLayout;
        }
        // Creates a layout with an icon, label, and view
        private StackLayout CreateViewWithIcon(string label, View view, string iconImage)
        {
            var icon = new Image
            {
                Source = iconImage,
                WidthRequest = 16,
                HeightRequest = 16
            };

            var labelView = new Label
            {
                Text = label,
                TextColor = Color.Black,
                FontSize = 20,
                VerticalOptions = LayoutOptions.Center
            };

            var layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Children = { icon, labelView, view },
                BackgroundColor = Color.LightGray
            };

            return layout;
        }
        // Returns the image file name based on the gender value
        private string GetGenderIconImage(Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return "male.png";
                case Gender.Female:
                    return "female.png";
                default:
                    return "human.png";
            }
        }
        // Event handler for the save button click
        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            // Update user information based on the input fields
            user.Name = nameEntry.Text;
            user.BirthDate = birthDateDatePicker.Date.Date.ToString("yyyy-MM-dd");
            user.Gender = (Gender)Enum.Parse(typeof(Gender), genderPicker.SelectedItem.ToString());
            user.Bio = bioEditor.Text;

            // Call the Save method to save the updated user information
            bool isOk = await Save(user);

            // Display a success or failure message based on the save operation
            if (isOk)
            {
                await DisplayAlert("Erfolg", "Profil erfolgreich aktualisiert!", "OK");
            }
            else
            {
                await DisplayAlert("Fehlschlag", "Profil wurde nicht aktualisiert!", "OK");
            }

            // Navigate back to the previous page
            await Navigation.PopAsync();
        }

        private async Task<bool> Save(User user)
        {
            string token = LoginPage.token;
            HttpClientHandler insecureHandler = Registration.GetInsecureHandler();
            using (var client = new HttpClient(insecureHandler))
            {
                client.BaseAddress = new Uri("https://10.0.2.2:7178/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                User updatedUser = new User
                {
                    Id = user.Id,
                    //Avatar = "placeholder_avatar.png",
                    Role = user.Role,
                    Username = user.Username,
                    Name = user.Name,
                    Email = user.Email,
                    Gender = user.Gender,
                    BirthDate = user.BirthDate,
                    Followers = user.Followers,
                    Following = user.Following,
                    CreatedAt = user.CreatedAt,
                    Bio = user.Bio,
                    Interests = user.Interests,
                    Locked = user.Locked
                };
                LoginPage.currentUser = updatedUser;

                string jsonPayload = JsonConvert.SerializeObject(updatedUser);
                updatedUser.Avatar = "placeholder_avatar.png";
                HttpResponseMessage response = await client.PutAsync("api/User/" + user.Id, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
