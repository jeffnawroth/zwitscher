using System;
using System.Collections.Generic;
using System.Linq;

namespace App3.Services
{
    public class DummyBackend
    {
        private static List<User> registeredUsers;
        private static List<Post> postings;
        private static List<Post> dummyPostings;

        static DummyBackend()
        {
            registeredUsers = new List<User>();
            postings = new List<Post>();
            dummyPostings = new List<Post>();
        }

        public static bool IsUserRegistered(string email)
        {
            return registeredUsers.Any(user => user.Email == email);
        }

        public static bool SaveRegistrationData(string name, int id, string email, string username, string password)
        {
            if (IsUserRegistered(email))
                return false;

            var user = new User
            {
                Name = name,
                ID = id,
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

        public static void SavePosting(int userId, string Avatar, string content)
        {
            // Erstelle ein neues Posting-Objekt
            var posting = new Post
            {
                PostId = GeneratePostId(),
                UserId = userId,
                UserAvatar = Avatar,
                ThumbsUpUserIds = new int[0],
                ThumbsDownUserIds = new int[0],
                Timestamp = DateTime.Now
            };

            // Speichere das Posting
            dummyPostings.Add(posting);
        }




        public static List<User> GetRegisteredUsers()
        {
            return registeredUsers;
        }

        public static List<Post> GetPostings()
        {
            return postings;
        }
        private static int postIdCounter = 12543;

        private static int GeneratePostId()
        {
            // Generiere eine eindeutige Post-ID
            // Hier kann ein geeigneter Algorithmus verwendet werden, um eine eindeutige ID zu generieren
            // Zum Beispiel kann eine Zählervariable verwendet werden, die bei jedem Aufruf der Methode inkrementiert wird
            // Oder Sie können eine externe Bibliothek zur Generierung von eindeutigen IDs verwenden
            // In diesem Beispiel verwenden wir eine einfache Zählerlogik

            // Inkrementiere den Post-ID-Zähler
            postIdCounter++;

            // Gib die generierte Post-ID zurück
            return postIdCounter;
        }
    }
}
