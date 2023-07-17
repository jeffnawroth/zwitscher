using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            // set layout 
            InitializeComponent();
        }
        // buttons to navigate to the diffrent pages when clicked
        private void UsersettingsButton_Clicked(object sender, EventArgs e) 
        {
            Navigation.PushAsync(new UserSettings());
        }

        private void NotificationButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Notification());
        }

        private void DeletButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DeletAccount());
        }
    }
}
