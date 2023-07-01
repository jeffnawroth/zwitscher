using App3;
using App3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3

{
    public partial class CustomApp : Application
    {
        private User loggedInUser;
        public CustomApp(User loggedInUser)
        {
            InitializeComponent();

            this.loggedInUser = loggedInUser; // Den angemeldeten Benutzer speichern

            // Überprüfen, ob ein Benutzer eingeloggt ist
            var isLoggedIn = Settings.IsLoggedIn;

            if (isLoggedIn)
            {
                // Erstellen Sie eine NavigationPage und setzen Sie die AppShell als RootPage
                MainPage = new NavigationPage(new AppShell(loggedInUser));
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }
        protected override void OnStart()
        {
        }
        protected override void OnSleep() 
        {
        }
        protected override void OnResume()
        {
        }
            private User GetUserFromDatabase(User loggedInUser)
        {
            // Code zum Abrufen des Benutzers aus Ihrer Datenbank
            // Beispiel:
            List<User> registeredUsers = Database.GetRegisteredUsers(); // Annahme: GetRegisteredUsers gibt die Liste der registrierten Benutzer zurück

            // Den angemeldeten Benutzer auswählen
            User user = registeredUsers.FirstOrDefault(u => u.Id == loggedInUser.Id);

            return user;
        }


    }

    // Restlicher Code...

}

