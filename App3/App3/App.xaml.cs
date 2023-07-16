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

            this.loggedInUser = loggedInUser; // save logged in user

            // check if user is logged in 
            var isLoggedIn = Settings.IsLoggedIn;

            if (isLoggedIn)
            {
               // Create a NavigationPage and set the AppShell as the RootPage
                MainPage = new NavigationPage(new AppShell(loggedInUser));
            }
            else
            {
                // create a nagigationPage and set as logínpage 
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
    }

}

