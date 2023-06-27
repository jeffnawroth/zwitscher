using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3
{
    public partial class CustomApp : Xamarin.Forms.Application
    {
        public CustomApp()
        {
            InitializeComponent();

            if (Settings.IsLoggedIn)
            {
                MainPage = new NavigationPage(new AppShell());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart()
        {
            // Hier können Sie Initialisierungslogik für den Start der App hinzufügen
            InitializeComponent();

            if (Settings.IsLoggedIn)
            {
                MainPage = new NavigationPage(new AppShell());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnSleep()
        {
            // Hier können Sie Logik für den Zustand "Schlafend" der App hinzufügen
        }

        protected override void OnResume()
        {
            // Hier können Sie Logik für den Zustand "Wiederaufnahme" der App hinzufügen
        }
    }
}
