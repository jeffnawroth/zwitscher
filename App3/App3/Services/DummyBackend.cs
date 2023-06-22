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
            private static int userIdCounter;

            static DummyBackend()
            {
                registeredUsers = new List<User>();
                postings = new List<Post>();
                dummyPostings = new List<Post>();
                userIdCounter = 0;
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
                    Id = GenerateUserId(),
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
                    // Check if the password matches
                    return user.Password == password;
                }
                return false;
            }

            public static void SavePosting(int userId, string avatar, string content)
            {
                // Create a new posting object
                var posting = new Post
                {
                    PostId = GeneratePostId(),
                    UserId = userId,
                    UserAvatar = avatar,
                    ThumbsUpUserIds = new List<int>(),
                    ThumbsDownUserIds = new List<int>(),
                    Content = content,
                    Timestamp = DateTime.Now
                };

                // Save the posting
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

            private static int postIdCounter = 0;

            private static int GeneratePostId()
            {
                // Generate a unique post ID
                // Here, you can use a suitable algorithm to generate a unique ID
                // For example, you can use a counter variable that increments on each method call
                // Or you can use an external library for generating unique IDs
                // In this example, we are using a simple counter logic

                // Increment the post ID counter
                postIdCounter++;

                // Return the generated post ID
                return postIdCounter;
            }

            private static int GenerateUserId()
            {
                // Generate a unique user ID
                // Here, we are using a simple counter logic to generate an incrementing ID
                // Replace this with your own logic for generating unique user IDs
                userIdCounter++;

                // Return the generated user ID
                return userIdCounter;
            }
        }
    }


