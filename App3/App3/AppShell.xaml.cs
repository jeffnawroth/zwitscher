using Xamarin.Forms;

namespace App3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            FlyoutItem searchItem = new FlyoutItem
            {
                Title = "Suche",
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem
            };

            ShellSection searchSection = new ShellSection
            {
                Title = "Suche",
                Icon = "search_icon.png"
            };
            ShellContent searchContent = new ShellContent
            {
                ContentTemplate = new DataTemplate(typeof(Search))
            };
            searchSection.Items.Add(searchContent);
            searchItem.Items.Add(searchSection);
            Items.Add(searchItem);
        }
    }
}