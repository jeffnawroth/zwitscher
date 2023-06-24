using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace App3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostingPopUp : PopupPage
    {
        public PostingPopUp()
        {
            InitializeComponent();
        }

        private async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}
