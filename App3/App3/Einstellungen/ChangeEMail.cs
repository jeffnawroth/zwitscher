using System;
using Xamarin.Forms;

namespace App3
{
    public class ChangeEMail: ContentPage
    {
        private Entry oldEmailEntry;
        private Entry newEmailEntry;
        private Entry confirmEmailEntry;

        public ChangeEMail()
        {
            Title = "E-Mail ändern";

            oldEmailEntry = new Entry
            {
                Placeholder = "Alte E-Mail",
                PlaceholderColor = Color.Black,
                TextColor = Color.Black,
            };

            newEmailEntry = new Entry
            {
                Placeholder = "Neue E-Mail",
                PlaceholderColor = Color.Black,
                TextColor = Color.Black,
            };

            confirmEmailEntry = new Entry
            {
                Placeholder = "E-Mail bestätigen",
                PlaceholderColor = Color.Black,
                TextColor = Color.Black,
            };

            var changeButton = new Button
            {
                Text = "E-Mail ändern"
            };
            changeButton.Clicked += OnChangeEmailButtonClicked;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    oldEmailEntry,
                    newEmailEntry,
                    confirmEmailEntry,
                    changeButton
                }
            };
        }

        private void OnChangeEmailButtonClicked(object sender, EventArgs e)
        {
            string oldEmail = oldEmailEntry.Text;
            string newEmail = newEmailEntry.Text;
            string confirmEmail = confirmEmailEntry.Text;

            // Führe die Überprüfungen durch und ändere die E-Mail-Adresse, falls gültig
            if (CheckOldEmail(oldEmail) && CheckNewEmailsMatch(newEmail, confirmEmail))
            {
                ChangeEmail(oldEmail, newEmail);

                DisplayAlert("Erfolg", "E-Mail-Adresse erfolgreich geändert!", "OK");

                // Zurücksetzen der Eingabefelder
                oldEmailEntry.Text = string.Empty;
                newEmailEntry.Text = string.Empty;
                confirmEmailEntry.Text = string.Empty;
            }
            else
            {
                DisplayAlert("Fehler", "Falsche E-Mail-Adresse oder neue E-Mail-Adressen stimmen nicht überein.", "OK");
            }
        }


        private bool CheckOldEmail(string email)
        {
            // Hier kannst du die Überprüfung der alten E-Mail-Adresse implementieren
            // Rückgabe des Ergebnisses der Überprüfung
            return true;
        }

        private bool CheckNewEmailsMatch(string newEmail, string confirmEmail)
        {
            // Hier kannst du die Überprüfung übereinstimmender neuer E-Mail-Adressen implementieren
            // Rückgabe des Ergebnisses der Überprüfung
            return newEmail == confirmEmail;
        }

        private void ChangeEmail(string oldEmail, string newEmail)
        {
            // Hier kannst du die Logik zum tatsächlichen Ändern der E-Mail-Adresse implementieren
        }
    }
}
