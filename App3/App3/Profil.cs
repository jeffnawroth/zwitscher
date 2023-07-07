using App3.Layouts;
using App3.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App3
{
    public class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            var profileLayout = ProfilePageLayout.CreateProfilePageLayout(LoginPage.currentUser);

            Content = new StackLayout
            {
                Children = { profileLayout }
            };
        }
    }
}
