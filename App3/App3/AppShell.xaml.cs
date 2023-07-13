using App3.Services;
using Xamarin.Forms;

namespace App3
{
    public partial class AppShell : Shell
    {
        public User LoggedInUser { get; set; }
        public AppShell(User currentUser)
        {
            InitializeComponent();
        }
    }
}
