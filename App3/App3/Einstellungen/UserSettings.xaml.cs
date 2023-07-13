using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSettings : ContentPage
    {
        public UserSettings()
        {
            InitializeComponent();
        }
        private void OnChangePasswordButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangePasswordPage());
        }

        private void OnChangeEmailButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangeEMail());
        }
    }
}