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
        private User GetUserFromDatabase()
        {
            // Code zum Abrufen des Benutzers aus Ihrer Datenbank
            // Beispiel:
            List<User> registeredUsers = Database.GetRegisteredUsers(); // Annahme: GetRegisteredUsers gibt die Liste der registrierten Benutzer zurück

            // Hier können Sie den gewünschten Benutzer auswählen, z. B. anhand der ID oder einer anderen Eigenschaft
            User user = registeredUsers.FirstOrDefault(u => u.Id == 1); // Annahme: Den Benutzer mit der ID 1 auswählen

            return user;
        }

    }
}
