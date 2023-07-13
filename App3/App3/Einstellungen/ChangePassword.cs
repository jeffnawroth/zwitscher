using System;
using Xamarin.Forms;

namespace App3
{
    public class ChangePasswordPage : ContentPage
    {
        private Entry oldPasswordEntry;
        private Entry newPasswordEntry;
        private Entry confirmPasswordEntry;

        public ChangePasswordPage()
        {
            Title = "Passwort ändern";

            oldPasswordEntry = new Entry
            {
                Placeholder = "Altes Passwort",
                IsPassword = true,
                PlaceholderColor = Color.Black,
                TextColor = Color.Black
            };

            newPasswordEntry = new Entry
            {
                Placeholder = "Neues Passwort",
                IsPassword = true,
                PlaceholderColor = Color.Black,
                TextColor = Color.Black
            };

            confirmPasswordEntry = new Entry
            {
                Placeholder = "Passwort bestätigen",
                IsPassword = true,
                PlaceholderColor = Color.Black,
                TextColor = Color.Black
            };

            var changeButton = new Button
            {
                Text = "Passwort ändern",
                TextColor = Color.Black,
                BackgroundColor = Color.LightGray
            };
            changeButton.Clicked += OnChangePasswordButtonClicked;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    oldPasswordEntry,
                    newPasswordEntry,
                    confirmPasswordEntry,
                    changeButton
                }
            };
        }

        private void OnChangePasswordButtonClicked(object sender, EventArgs e)
        {
            string oldPassword = oldPasswordEntry.Text;
            string newPassword = newPasswordEntry.Text;
            string confirmPassword = confirmPasswordEntry.Text;

            // Führe die Überprüfungen durch und ändere das Passwort, falls gültig
            if (CheckOldPassword(oldPassword) && CheckNewPasswordsMatch(newPassword, confirmPassword))
            {
                // Ändere das Passwort
                ChangePassword(oldPassword, newPassword);

                // Zeige eine Erfolgsmeldung an
                DisplayAlert("Erfolg", "Passwort erfolgreich geändert!", "OK");

                // Zurücksetzen der Eingabefelder
                oldPasswordEntry.Text = string.Empty;
                newPasswordEntry.Text = string.Empty;
                confirmPasswordEntry.Text = string.Empty;
            }
            else
            {
                // Zeige eine Fehlermeldung an
                DisplayAlert("Fehler", "Falsches Passwort oder neue Passwörter stimmen nicht überein.", "OK");
            }
        }

        private bool CheckOldPassword(string password)
        {
            // Hier kannst du die Überprüfung des alten Passworts implementieren
            // Rückgabe des Ergebnisses der Überprüfung
            return true;
        }

        private bool CheckNewPasswordsMatch(string newPassword, string confirmPassword)
        {
            // Hier kannst du die Überprüfung übereinstimmender neuer Passwörter implementieren
            // Rückgabe des Ergebnisses der Überprüfung
            return newPassword == confirmPassword;
        }

        private void ChangePassword(string oldPassword, string newPassword)
        {
            // Hier kannst du die Logik zum tatsächlichen Ändern des Passworts implementieren
        }
    }
}
