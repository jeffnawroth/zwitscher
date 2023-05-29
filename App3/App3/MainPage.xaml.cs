using App3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Erstelle eine Instanz der AppShell
            var appShell = new AppShell();

            // Setze die Hauptnavigation auf die AppShell
            MainPage = appShell;
        }
    }
}