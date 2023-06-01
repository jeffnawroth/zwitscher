using System;
using System.Collections.Generic;
using System.Linq;

namespace App3.Services
{
    public class DummyBackend
    {
        private static List<User> registeredUsers;

        static DummyBackend()
        {
            registeredUsers = new List<User>();
        }

        public static bool IsUserRegistered(string email)
        {
            return registeredUsers.Any(user => user.Email == email);
        }

        public static bool SaveRegistrationData(string vorname, string nachname, string email, string username, string password)
        {
            if (IsUserRegistered(email))
                return false;

            var user = new User
            {
                Vorname = vorname,
                Nachname = nachname,
                Email = email,
                Username = username,
                Password = password
            };

            registeredUsers.Add(user);
            return true;
        }
        public static bool ValidatePassword(string email, string password)
        {
            User user = registeredUsers.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                // Überprüfen, ob das Passwort übereinstimmt
                return user.Password == password;
            }
            return false;
        }


        public static List<User> RegisteredUsers
        {
            get { return registeredUsers; }
        }
    }
}
