using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;


namespace App3
{
    internal partial class Settings
    {
        public static bool IsLoggedIn
        {
            get => Preferences.Get(nameof(IsLoggedIn), false);
            set => Preferences.Set(nameof(IsLoggedIn), value);
        }
    }
}
