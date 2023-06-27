using Xamarin.Forms;

namespace App3
{
    public class Logout : ContentPage
    {
        public Logout()
        {
            var logoutButton = new Button
            {
                Text = "Logout",
                Command = new Command(OnLogoutButtonClicked)
            };

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = { logoutButton }
            };
        }

        private async void OnLogoutButtonClicked()
        {
            // Setzen Sie den IsLoggedIn-Status auf false
            Settings.IsLoggedIn = false;

            // Navigieren Sie zur LoginPage
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }
}
