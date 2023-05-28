using Xamarin.Forms;
using System.Threading.Tasks;

namespace App3
{
    public static class LoginService
    {
        // asynchron
        public static async Task<bool> ValidateCredentials(string username, string password)
        {
            // registration option from data
            // Datastruction implement

         
            await Task.Delay(2000);

            // dummy
            if (username == "admin" && password == "password")
            {
                return true;
            }

            return false;
        }
    }
}
