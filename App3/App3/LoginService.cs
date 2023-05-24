using Xamarin.Forms;

namespace App3
{
    public static class LoginService
    {
        public static bool ValidateCredentials(string username, string password)
        {
            // When backend ready do the structure
            return username == "admin" && password == "password";
        }
    }
}
