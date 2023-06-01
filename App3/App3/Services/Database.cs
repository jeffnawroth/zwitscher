using System.Collections.Generic;

using System.Linq;

namespace App3.Services
{
    public class Database
    {
        private static List<User> registeredUsers;

        static Database()
        {
            registeredUsers = new List<User>();
        }

        public static bool IsUserRegistered(string email)
        {
            return registeredUsers.Any(user => user.Email == email);
        }

        public static bool SaveRegistrationData(User user)
        {
            if (IsUserRegistered(user.Email))
                return false;

            registeredUsers.Add(user);
            return true;
        }

        public static List<User> GetRegisteredUsers()
        {
            return registeredUsers;
        }
    }
}
