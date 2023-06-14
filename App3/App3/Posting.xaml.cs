using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3
{
    public partial class Posting : ContentPage
    {
        private MainPage mainPage;

        public Posting(MainPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
        }

        private void PublishButton_Clicked(object sender, EventArgs e)
        {
            string postText = postingText.Text;
            MainPage mainPage = new MainPage(); 
            mainPage.AddPost(postText); 
            Navigation.PopAsync();
        }
    }
}
