using System.Linq;
using System.Threading.Tasks;
using App3.Services;

namespace App3.Services
{
    public static class LoginService
    {
        public static User CurrentUser { get; private set; }

        public static void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        public static async Task<bool> ValidateCredentials(string username, string password)
        {
            await Task.Delay(2000);

            bool isUserRegistered = DummyBackend.IsUserRegistered(username);
            bool isPasswordValid = DummyBackend.ValidatePassword(username, password);

            bool isLoginValid = isUserRegistered && isPasswordValid;
            if (isLoginValid)
            {
                User user = DummyBackend.GetRegisteredUsers().FirstOrDefault(u => u.Username == username);
                SetCurrentUser(user);
            }
            return isLoginValid;
        }
    }
}