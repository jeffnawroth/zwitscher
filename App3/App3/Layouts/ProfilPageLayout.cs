using App3.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;
using System;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Pages;

using System.Threading.Tasks;

namespace App3.Layouts
{
    public class ProfilePageLayout
    {

        public static StackLayout Content { get; private set; }
        // takes User object as a paramenter and create/ return an Stacklayout
        public static StackLayout CreateProfilePageLayout(User user)
        {
            // create the mainlayout
            var mainLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Padding = new Thickness(10),
                BackgroundColor = Color.White,
            };
            // create a button to edit the profil
            var editButton = new ImageButton
            {
                Source = "edit_profile.png",
                WidthRequest = 20,
                HeightRequest = 20,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            mainLayout.Children.Add(editButton);

            editButton.Clicked += (sender, e) =>
            {
                EditProfile(user);
            };
            // create UserAvatar Image
            var avatarImage = new Image
            {
                Source = user.Avatar,
                WidthRequest = 180,
                HeightRequest = 180,
                // Aspect = Aspect.AspectFill,
                BackgroundColor = Color.Transparent
            };
            mainLayout.Children.Add(avatarImage);
            // create nameLabel
            var nameLabel = new Label
            {
                Text = user.Name,
                TextColor = Color.Black,
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            mainLayout.Children.Add(nameLabel);
            // create usernameLabel
            var usernameLabel = new Label
            {
                Text = "@" + user.Username,
                FontSize = 24,
                TextColor = Color.Black

            };
            // create followers and followings, write from array to counter
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
            // followingLayout containing username, followings and followers
            var followersLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Children = { usernameLabel, followersLabel, followingLabel },
                HorizontalOptions = LayoutOptions.Center
            };
            mainLayout.Children.Add(followersLayout);
            // create info Layout 
            var infoLayout = new FlexLayout
            {
                Direction = FlexDirection.Row,
                Wrap = FlexWrap.Wrap,
                JustifyContent = FlexJustify.SpaceBetween,
                AlignItems = FlexAlignItems.Center,
                AlignContent = FlexAlignContent.Center,
                Margin = new Thickness(0, 10)
            };
            // create birthdaylabel out of users Birthdate 
            var birthDayLabel = new Label
            {
                Text = $"{user.BirthDate}",
                TextColor = Color.Black,
                FontSize = 20
            };
            var birthDateLayout = CreateViewWithIcon("geboren: ", birthDayLabel, "birthday.png");
            infoLayout.Children.Add(birthDateLayout);
            // create joindatelabel
            var joinDateLabel = new Label
            {
                Text = $"{user.CreatedAt.ToString("dd/MM/yyyy")}",
                TextColor = Color.Black,
                FontSize = 20
            };
            var joinDateLayout = CreateViewWithIcon("beigetreten: ", joinDateLabel, "startday.png");
            infoLayout.Children.Add(joinDateLayout);

            // create generLabel based on users gender
            var genderLabel = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };
            var genderImage = new Image { };


            var genderLayout = CreateViewWithIcon("Gender", genderLabel, GetGenderIconImage(user.Gender.Value));
            infoLayout.Children.Add(genderLayout);
            mainLayout.Children.Add(infoLayout);
            // checking users gender and return fitting texts
            if (user.Gender == Gender.Male)
            {

                genderLabel.Text = "Männlich";
            }
            else if (user.Gender == Gender.Female)
            {

                genderLabel.Text = "Weiblich";
            }
            else
            {

                genderLabel.Text = "Divers";
            }
            // create interestslabel
            var interestsLabel = new Label
            {
                Text = "Interessen",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.Center
            };
            mainLayout.Children.Add(interestsLabel);
            // create interests Layout 
            var interestsLayout = new FlexLayout
            {
                Wrap = FlexWrap.Wrap,
                JustifyContent = FlexJustify.SpaceBetween,
                AlignItems = FlexAlignItems.Start,
                AlignContent = FlexAlignContent.Start,
                Margin = new Thickness(0, 5)
            };
            // Adds each interest label as a child to the FlexLayout
            foreach (var interest in user.Interests)
            {
                var interestLabel = new Label
                {
                    Text = interest,
                    TextColor = Color.Black,
                    BackgroundColor = Color.LightGray,
                    FontSize = 24,

                };
                interestsLayout.Children.Add(interestLabel);
            };

            mainLayout.Children.Add(interestsLayout);
            // create biolabel
            var bioLabel = new Editor
            {
                Text = user.Bio,
                TextColor = Color.Black,
                FontSize = 14,
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            mainLayout.Children.Add(bioLabel);

            var separatorLine = new BoxView
            {
                BackgroundColor = Color.Black,
                HeightRequest = 1
            };

            mainLayout.Children.Add(separatorLine);

            Content = mainLayout;
            // giving back the layout 
            return mainLayout;

        }
        private static async void EditProfile(User user)
        {
            // Create a new instance of the EditProfilePage and pass the user object
            var editPage = new EditProfilePage(user);

            // Navigate to the EditProfilePage
            await Application.Current.MainPage.Navigation.PushAsync(editPage);
        }
        private static StackLayout CreateViewWithIcon(string label, View view, string iconImage)
        {
            // Create an Image view for the icon
            var icon = new Image
            {
                Source = iconImage,
                WidthRequest = 16,
                HeightRequest = 16
            };

            // Create a Label view for the label text
            var labelView = new Label
            {
                Text = label,
                TextColor = Color.Black,
                FontSize = 20,
                VerticalOptions = LayoutOptions.Center
            };

            // Create a StackLayout to contain the icon, label, and view
            var layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                Children = { icon, labelView, view },
                BackgroundColor = Color.LightGray
            };
            // giving back the layout
            return layout;
        }

        private static string GetGenderIconImage(Gender gender)
        {
            // Use a switch statement to determine the gender and return the corresponding image file name
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

    }
}