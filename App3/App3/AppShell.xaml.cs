using System;
using Xamarin.Forms;

namespace App3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // FlyoutItem für die MainPage hinzufügen
            FlyoutItem mainPageItem = new FlyoutItem
            {
                Title = "Startseite",
                Icon = "home_icon.png",
                Route = "main",
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem
            };

            // MainPage zur Shell hinzufügen
            mainPageItem.Items.Add(new ShellContent
            {
                ContentTemplate = new DataTemplate(typeof(MainPage))
            });

            // Hamburgermenü zur Shell hinzufügen
            Items.Add(mainPageItem);

            // Weitere FlyoutItems hinzufügen
            FlyoutItem searchItem = new FlyoutItem
            {
                Title = "Suche",
                Icon = "search_icon.png",
                Route = "search"
            };
            searchItem.Items.Add(new ShellContent
            {
                ContentTemplate = new DataTemplate(typeof(Search))
            });

            FlyoutItem profileItem = new FlyoutItem
            {
                Title = "Profil",
                Icon = "profile_icon.png",
                Route = "profile"
            };
            profileItem.Items.Add(new ShellContent
            {
                ContentTemplate = new DataTemplate(typeof(Profil))
            });

            // FlyoutItems zur Shell hinzufügen
            Items.Add(searchItem);
            Items.Add(profileItem);

            FlyoutItem logoutItem = new FlyoutItem
            {
                Title = "Logout",
                Icon = "logout_icon.png",
                Route = "logout"
            };
            logoutItem.Items.Add(new ShellContent
            {
                ContentTemplate = new DataTemplate(typeof(Logout))
            });

            Items.Add(searchItem);
            Items.Add(profileItem);
            Items.Add(logoutItem);
            // Routen registrieren
            Routing.RegisterRoute("main", typeof(MainPage));
            Routing.RegisterRoute("search", typeof(Search));
            Routing.RegisterRoute("profile", typeof(Profil));
            Routing.RegisterRoute("logout", typeof(Logout));

            
        }
    }
}
