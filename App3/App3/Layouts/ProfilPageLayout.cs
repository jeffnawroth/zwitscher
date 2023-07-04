using App3.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;

namespace App3.Layouts
{
    public class ProfilePageLayout
    {
        public static StackLayout Content { get; private set; }

        public static StackLayout CreateProfilePageLayout(User user)
        {

            var mainLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Padding = new Thickness(10),
                BackgroundColor = Color.White,
            };
            var userGrid = new Grid();
            userGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            userGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            userGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            userGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            userGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var avatar = new Image
            {
                Source = user.Avatar,
                WidthRequest = 90,
                HeightRequest = 90,
                //Aspect = Aspect.AspectFill
            };
            Grid.SetRowSpan(avatar, 3);
            userGrid.Children.Add(avatar);
            var nameLabel = new Label
            {
                Text = user.Name,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            };
            Grid.SetRow(nameLabel, 0);
            Grid.SetColumn(nameLabel, 1);
            userGrid.Children.Add(nameLabel);
            var usernameLabel = new Label
            {
                Text = "@" + user.Username,
                FontSize = 14,
                TextColor = Color.Black
            };
            Grid.SetRow(usernameLabel, 1);
            Grid.SetColumn(usernameLabel, 1);
            userGrid.Children.Add(usernameLabel);

            var followersLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 10
            };
            var followersLabel = new Label
            {
                
                Text = user.Followers.Count + " Followers",
                TextColor = Color.Black,
                FontSize = 14
            };
            var followingsLabel = new Label
            {
                Text = user.Following.Count + " Folge ich",
                TextColor = Color.Black,
                FontSize = 14
            };

            followersLayout.Children.Add(followersLabel);
            followersLayout.Children.Add(followingsLabel);

            Grid.SetRow(followersLayout, 2);
            Grid.SetColumn(followersLayout, 1);
            userGrid.Children.Add(followersLayout);

            var editButton = new ImageButton
            {
                Source = "edit_profile.png",
                WidthRequest = 20,
                HeightRequest = 20,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            Grid.SetColumnSpan(editButton, 1);
            Grid.SetRow(editButton, 0);
            Grid.SetRowSpan(editButton, 2);
            Grid.SetColumn(editButton, 1);
            userGrid.Children.Add(editButton);
          

            var birthDateLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 5
            };
            var birthDateImage = new Image { Source = "birthday.png", WidthRequest = 20, HeightRequest = 20 };
            var birthDateLabel = new Label { Text = "geboren: " + user.BirthDate.ToString("d"), TextColor = Color.Black, FontSize = 9};
            birthDateLayout.Children.Add(birthDateImage);
            birthDateLayout.Children.Add(birthDateLabel);

            var joinDateLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 5
            };
            var joinDateImage = new Image { Source = "startday.png", WidthRequest = 20, HeightRequest = 20 };
            var joinDateLabel = new Label { Text = "beigetreten: " + user.CreatedAt.ToString("d"), TextColor = Color.Black, FontSize = 9 };
            joinDateLayout.Children.Add(joinDateImage);
            joinDateLayout.Children.Add(joinDateLabel);

            var genderImage = new Image
            {
                WidthRequest = 20,
                HeightRequest = 20,
            };
            
            var genderLabel = new Label
            {
                TextColor = Color.Black,
                FontSize = 9
            }; 

            if (user.Gender == Gender.Male)
            {
                genderImage.Source = "male.png";
                genderLabel.Text = "Männlich";
            }
            else if (user.Gender == Gender.Female)
            {
                genderImage.Source = "female.png";
                genderLabel.Text = "Weiblich";
            }
            else
            {
                genderImage.Source = "human.png";
                genderLabel.Text = "Divers";
            }

            var genderLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 5,
                Children = { genderImage, genderLabel }
            };
            var birthDateFrame = new Frame
            {
                BackgroundColor = Color.LightGray,
                CornerRadius = 5,
                Padding = new Thickness(15),
                Content = birthDateLayout
            };
            var joinDateFrame = new Frame
            {
                BackgroundColor = Color.LightGray,
                CornerRadius = 5,
                Padding = new Thickness(15),
                Content = joinDateLayout
            };
            var genderFrame = new Frame
            {
                BackgroundColor = Color.LightGray,
                CornerRadius = 5,
                Padding = new Thickness(15),
                Content = genderLayout
            };
            var additionalInfoGrid = new Grid();
            additionalInfoGrid.RowDefinitions.Add(new RowDefinition { Height = 30 }); // Zeile 0
            additionalInfoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Spalte 0
            additionalInfoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Spalte 1
            additionalInfoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Spalte 2

            Grid.SetRow(birthDateFrame, 0); // Zeile 0
            Grid.SetColumn(birthDateFrame, 0); // Spalte 0

            Grid.SetRow(joinDateFrame, 0); // Zeile 0
            Grid.SetColumn(joinDateFrame, 1); // Spalte 1

            Grid.SetRow(genderFrame, 0); // Zeile 0
            Grid.SetColumn(genderFrame, 2); // Spalte 2

            additionalInfoGrid.Children.Add(birthDateFrame);
            additionalInfoGrid.Children.Add(joinDateFrame);
            additionalInfoGrid.Children.Add(genderFrame);
            // Annahme: Die Interessen sind in einer Liste von Strings namens "interests" enthalten

            // Annahme: Die Interessen sind in einer Liste von Strings namens "interests" enthalten

            var interestsGrid = new Grid();
            interestsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Zeile 0

            int row = 0;
            int column = 0;
            for (int i = 0; i < user.Interests.Count; i++)
            {
                var interest = user.Interests[i];

                var interestLabel = new Label
                {
                    Text = interest,
                   
                    TextColor = Color.Black,
                    FontSize = 10,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                   
                };
                var interestFrame = new Frame
                {
                    Content = interestLabel,
                    BackgroundColor = Color.White,
                    BorderColor = Color.Black,
                    CornerRadius = 5,
                    
                };


                interestsGrid.Children.Add(interestFrame, column, row);

                column++;
                if (column == 3)
                {
                    interestsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    row++;
                    column = 0;
                }
            }
            var bioLabel = new Label
            {
                Text = user.Bio,
                TextColor = Color.Black,
                FontSize = 12,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(5)
            };
            var separatorLine = new BoxView
            {
                BackgroundColor = Color.Black,
                HeightRequest = 1
            };

          

            // Fügen Sie das additionalInfoLayout zu Ihrem Hauptlayout hinzu
            mainLayout.Children.Add(userGrid);
            mainLayout.Children.Add(additionalInfoGrid);
            mainLayout.Children.Add(interestsGrid);
            mainLayout.Children.Add(bioLabel);
            mainLayout.Children.Add(separatorLine);


            Content = mainLayout;


            // Fügen Sie hier den Code für weitere Informationen zum Profil hinzu, z. B. Biografie, Beiträge usw.

           

            return mainLayout;

        }
    }
}
