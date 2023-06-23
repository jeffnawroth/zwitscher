using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App3.Services;
using System.Linq;

namespace App3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        private User currentUser;

        public EditProfilePage(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadProfileData();
        }

        private void LoadProfileData()
        {
            // Load the current user's profile data into the input fields
            NameEntry.Text = currentUser.Name;
            BirthDateEntry.Date = currentUser.BirthDate.HasValue ? currentUser.BirthDate.Value : DateTime.Now;
            InterestsEntry.Text = string.Join(", ", currentUser.Interests);
        }



        private void SaveProfileChanges()
        {
            // Save the changes to the current user's profile
            currentUser.Name = NameEntry.Text;
            currentUser.BirthDate = BirthDateEntry.Date;
            currentUser.Interests = InterestsEntry.Text.Split(',').Select(s => s.Trim()).ToList();

        // Perform any additional save operations, such as updating the database or API

        // Display a success message
        DisplayAlert("Erfolg", "Profil erfolgreich aktualisiert.", "OK");
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            SaveProfileChanges();
        }
    }
}
