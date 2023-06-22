using Xamarin.Forms;
using System.Threading.Tasks;
using App3.Services;
using System;

namespace App3.Services
{
    public static class LoginService
    {
        public static async Task<bool> ValidateCredentials(string username, string password)
        {
            await Task.Delay(2000);

            bool isUserRegistered = DummyBackend.IsUserRegistered(username);
            bool isPasswordValid = DummyBackend.ValidatePassword(username, password);

            bool isLoginValid = isUserRegistered && isPasswordValid;
            return isLoginValid;
        }
    }
}





