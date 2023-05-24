using Xamarin.Forms;

namespace App3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            FlyoutItem homeItem = new FlyoutItem
            {
                Title = "Home",
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem
            };

            ShellSection homeSection = new ShellSection
            {
                Title = "Home",
                Icon = "home_icon.png"
            };
            ShellContent homeContent = new ShellContent
            {
                ContentTemplate = new DataTemplate(typeof(MainPage))
            };
            homeSection.Items.Add(homeContent);
            homeItem.Items.Add(homeSection);
            Items.Add(homeItem);
        }
    }
}