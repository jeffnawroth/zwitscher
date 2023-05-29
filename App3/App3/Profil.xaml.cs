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
	public partial class Profil : ContentView
	{
		public Profil ()
		{
			InitializeComponent ();

            var nameLabel = new Label
            {
                Text = "John Doe",
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center
            };

            var emailLabel = new Label
            {
                Text = "john.doe@example.com",
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center
            };

            var profileImage = new Image
            {
                Source = "profile_image.png",
                HeightRequest = 200,
                WidthRequest = 200,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var contentStackLayout = new StackLayout
            {
                Spacing = 20,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = { nameLabel, emailLabel, profileImage }
            };

            var scrollView = new ScrollView
            {
                Content = contentStackLayout
            };

            Content = scrollView;
        }
	}
}