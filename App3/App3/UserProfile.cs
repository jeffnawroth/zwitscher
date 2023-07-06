using App3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace App3
{
    public class UserProfile : ContentPage
    {
        private User _user;
        private Button _followButton;
        private App _app; // Database anbinden 
        public UserProfile(User user)
        {
            var mainLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Padding = new Thickness(10),
                BackgroundColor = Color.White,
            };
            _followButton = new Button
            {
                Text = GetFollowButtonText(),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            _followButton.Clicked += FollowButton_Clicked;

            mainLayout.Children.Add(_followButton);
            var avatarImage = new Image
            {
                Source = user.Avatar,
                WidthRequest = 180,
                HeightRequest = 180,
                // Aspect = Aspect.AspectFill,
                BackgroundColor = Color.Transparent
            };
            mainLayout.Children.Add(avatarImage);
            var nameLabel = new Label
            {
                Text = user.Name,
                TextColor = Color.Black,
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            mainLayout.Children.Add(nameLabel);

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



            var birthDayLabel = new Label
            {
                Text = $"{user.BirthDate.ToString("dd/MM.yyyy")}",
                TextColor = Color.Black,
                FontSize = 20
            };
            var birthDateLayout = CreateViewWithIcon("geboren: ", birthDayLabel, "birthday.png");
            infoLayout.Children.Add(birthDateLayout);

            var joinDateLabel = new Label
            {
                Text = $"{user.CreatedAt.ToString("dd/MM/yyyy")}",
                TextColor = Color.Black,
                FontSize = 20
            };
            var joinDateLayout = CreateViewWithIcon("beigetreten: ", joinDateLabel, "startday.png");
            infoLayout.Children.Add(joinDateLayout);


            var genderLabel = new Label
            {
                TextColor = Color.Black,
                FontSize = 20
            };
            var genderImage = new Image { };


            var genderLayout = CreateViewWithIcon("Gender", genderLabel, GetGenderIconImage(user.Gender));
            infoLayout.Children.Add(genderLayout);
            mainLayout.Children.Add(infoLayout);

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

        }
        private string GetFollowButtonText()
        {
            if (App.CurrentUser.Following.Contains(_user.Id))
                return "Entfolgen";
            else
                return "Folgen";
        }
        private void FollowButton_Clicked(object sender, EventArgs e)
        {
            if (App.CurrentUser.Following.Contains(_user.Id))
                UnfollowUser();
            else
                FollowUser();

            _followButton.Text = GetFollowButtonText();
        }
        private void FollowUser()
        {
            App.CurrentUser.Following.Add(_user.Id); // Hinzufügen der User-ID zu Following-Liste des aktuellen Benutzers
            _user.Followers.Add(App.CurrentUser.Id); // Hinzufügen der eigenen User-ID zur Followers-Liste des angezeigten Benutzers
        }

        private void UnfollowUser()
        {
            App.CurrentUser.Following.Remove(_user.Id); // Entfernen der User-ID aus Following-Liste des aktuellen Benutzers
            _user.Followers.Remove(App.CurrentUser.Id); // Entfernen der eigenen User-ID aus Followers-Liste des angezeigten Benutzers
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
        private static string GetGenderIconImage(Gender gender)
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
    }

}