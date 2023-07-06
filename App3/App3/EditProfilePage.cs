using App3.Services;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

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

            var mainLayout = new StackLayout
            {
                Spacing = 10,
                Padding = new Thickness(10),
                BackgroundColor = Color.White,
            };
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
                var pickResult = await MediaPicker.PickPhotoAsync();

                if (pickResult != null)
                {
                    user.Avatar = pickResult.FullPath;
                    avatarButton.Source = ImageSource.FromFile(pickResult.FullPath);
                }
            };
            mainLayout.Children.Add(avatarButton);

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

            var usernameLabel = new Label
            {
                Text = "@" + user.Username,
                FontSize = 24,
                TextColor = Color.Black

            };
            

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
            birthDateDatePicker = new DatePicker
            {
                TextColor = Color.Black,
                Format = "dd.MM.yyyy",
                MaximumDate = DateTime.Today,
                Date = user.BirthDate,
                FontSize = 20
            };
            var birthDateLayout = CreateViewWithIcon("geboren: ", birthDateDatePicker, "birthday.png");
            infoLayout.Children.Add(birthDateLayout);

            var joinDateLabel = new Label
            {
                Text = $"{user.CreatedAt.ToString("dd/MM/yyyy")}",
                TextColor = Color.Black,
                FontSize = 20
            };
            var joinDateLayout = CreateViewWithIcon("beigetreten: ", joinDateLabel, "startday.png");
            infoLayout.Children.Add(joinDateLayout);

            genderPicker = new Picker
            {
                TextColor = Color.Black,
                FontSize = 20
            };
            genderPicker.Items.Add("Male");
            genderPicker.Items.Add("Female");
            genderPicker.Items.Add("Other");
            genderPicker.SelectedItem = user.Gender.ToString();
            var genderLayout = CreateViewWithIcon("Gender", genderPicker, GetGenderIconImage(user.Gender));
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

            var interestsLayout = new FlexLayout
            {
                Wrap = FlexWrap.Wrap,
                JustifyContent = FlexJustify.SpaceBetween,
                AlignItems = FlexAlignItems.Start,
                AlignContent = FlexAlignContent.Start,
                Margin = new Thickness(0, 5)
            };

            foreach (var interest in user.Interests)
            {
                var interestButton = CreateInterestButton(interest);
                interestsLayout.Children.Add(interestButton);
            }

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
                string newInterest = await DisplayPromptAsync("Neues Interesse hinzufügen", "Geben Sie das neue Interesse ein", "Hinzufügen", "Abbrechen");

                if (!string.IsNullOrWhiteSpace(newInterest))
                {
                    var newInterestButton = CreateInterestButton(newInterest);
                    interestsLayout.Children.Insert(interestsLayout.Children.Count - 1, newInterestButton);
                    user.Interests.Add(newInterest);
                }
            };

            interestsLayout.Children.Add(addInterestButton);

            Button CreateInterestButton(string interest)
            {
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
                    bool deleteConfirmed = await DisplayAlert("Interesse löschen", "Möchten Sie das Interesse wirklich löschen?", "Ja", "Nein");

                    if (deleteConfirmed)
                    {
                        interestsLayout.Children.Remove(interestButton);
                        user.Interests.Remove(interest);
                    }
                };

                return interestButton;
            }

            mainLayout.Children.Add(interestsLayout);
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
                // Fügen Sie hier die Logik hinzu, um die neue Biografie zu speichern oder zu aktualisieren
            };

            mainLayout.Children.Add(bioEditor);
            

            Content = mainLayout;
        }

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

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            user.Name = nameEntry.Text;
            user.BirthDate = birthDateDatePicker.Date;
            user.Gender = (Gender)Enum.Parse(typeof(Gender), genderPicker.SelectedItem.ToString());
            user.Bio = bioEditor.Text;

            // Fügen Sie hier die Logik hinzu, um die Benutzerdaten zu speichern oder zu aktualisieren

            await DisplayAlert("Erfolg", "Profil erfolgreich aktualisiert!", "OK");
            await Navigation.PopAsync();
        }
    }
}
