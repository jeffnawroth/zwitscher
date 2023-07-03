using Xamarin.Forms;
using App3.Services;

namespace App3.Layouts
{
    public class PostPageLayout
    {
        public static Grid CreateLayout()
        {
            var layout = new Grid();

            // Hier kannst du das Layout für die Posting-Seite definieren

            // Beispiel: Ein Textfeld für den Post-Text
            var postingText = new Entry
            {
                Placeholder = "Gib deinen Beitrag ein",
                Keyboard = Keyboard.Default,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Beispiel: Ein Label für den Zeichenzähler
            var characterCount = new Label
            {
                Text = "0 Zeichen",
                HorizontalOptions = LayoutOptions.End
            };

            // Beispiel: Ein Button zum Posten
            var postButton = new Button
            {
                Text = "Posten",
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            // Beispiel: Das Layout mit den erstellten Elementen füllen
            layout.Children.Add(postingText);
            layout.Children.Add(characterCount);
            layout.Children.Add(postButton);

            // Beispiel: Das Layout mit 2 Zeilen definieren
            layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Beispiel: Die Elemente in das Layout positionieren
            Grid.SetRow(postingText, 0);
            Grid.SetRow(characterCount, 1);
            Grid.SetColumnSpan(characterCount, 2);
            Grid.SetRow(postButton, 1);

            return layout;
        }
    }
}
