using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Hier können Sie Ihre Login-Logik implementieren, z. B. eine Datenbankabfrage oder eine API-Anforderung, um die Anmeldeinformationen zu überprüfen.

            if (username == "admin" && password == "password")
            {
                // Erfolgreich eingeloggt
                DisplayAlert("Erfolgreich", "Anmeldung erfolgreich!", "OK");
                // Hier können Sie den Benutzer zur Hauptseite weiterleiten oder andere Aktionen durchführen.
            }
            else
            {
                // Fehlgeschlagene Anmeldung
                DisplayAlert("Fehler", "Ungültige Anmeldeinformationen!", "OK");
            }
        }
    }
}
