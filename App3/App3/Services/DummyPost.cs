using System;
using System.Collections.Generic;

namespace App3
{
    public class DummyPost
    {
        public static List<Posting> CreateDummyPosts()
        {
            // Hier kannst du Dummy-Postings erstellen und zurückgeben
            var dummyPosts = new List<Posting>
            {
                new Posting
                {
                    PostId = 1,
                    Avatar = "placeholder_avatar.png",
                    Name = "John Doe",
                    UserId = 1,
                    Username = "johndoe",
                    PostText = "Dies ist Beispiel-Post 1",
                    Likes = new List<int> { 1, 2, 3 },
                    Dislikes = new List<int> { 4, 5 },
                    CommentCount = new List<int> { 6, 7, 8 },
                    Timestamp = DateTime.Now
                },
                new Posting
                {
                    PostId = 2,
                    Avatar = "placeholder_avatar.png",
                    Name = "Jane Smith",
                    UserId = 2,
                    Username = "janesmith",
                    PostText = "Dies ist Beispiel-Post 2",
                    Likes = new List<int> { 9, 10 },
                    Dislikes = new List<int> { 11 },
                    CommentCount = new List<int> { 12, 13 },
                    Timestamp = DateTime.Now.AddDays(-1)
                },
                new Posting
                {
                    PostId = 11,
                    Avatar = "placeholder_avatar.png",
                    Name = "Laura Miller",
                    UserId = 11,
                    Username = "@lauramiller",
                    PostText = "Dies ist Beispiel-Post 11",
                    Likes = new List<int> { 61, 62 },
                    Dislikes = new List<int> { 63, 64 },
                    CommentCount = new List<int> { 65, 66 },
                    Timestamp = DateTime.Now.AddDays(-10)
                },
                new Posting
                {
                    PostId = 12,
                    Avatar = "placeholder_avatar.png",
                    Name = "Andrew Thompson",
                    UserId = 12,
                    Username = "andrewthompson",
                    PostText = "Dies ist Beispiel-Post 12",
                    Likes = new List<int> { 67, 68 },
                    Dislikes = new List<int> { 69 },
                    CommentCount = new List<int> { 70, 71, 72 },
                    Timestamp = DateTime.Now.AddDays(-11)
                },
                new Posting
                {
                    PostId = 13,
                    Avatar = "placeholder_avatar.png",
                    Name = "Emma Wilson",
                    UserId = 13,
                    Username = "@emmawilson",
                    PostText = "Dies ist Beispiel-Post 13",
                    Likes = new List<int> { 73 },
                    Dislikes = new List<int> { 74, 75 },
                    CommentCount = new List<int> { 76, 77 },
                    Timestamp = DateTime.Now.AddDays(-12)
                },
                new Posting
                {
                    PostId = 14,
                    Avatar = "placeholder_avatar.png",
                    Name = "Jacob Davis",
                    UserId = 14,
                    Username = "@jacobdavis",
                    PostText = "Dies ist Beispiel-Post 14",
                    Likes = new List<int> { 78, 79 },
                    Dislikes = new List<int> { 80 },
                    CommentCount = new List<int> { 81, 82, 83 },
                    Timestamp = DateTime.Now.AddDays(-13)
                },
                new Posting
                {
                    PostId = 15,
                    Avatar = "placeholder_avatar.png",
                    Name = "Olivia Moore",
                    UserId = 15,
                    Username = "@oliviamoore",
                    PostText = "Dies ist Beispiel-Post 15",
                    Likes = new List<int> { 84, 85 },
                    Dislikes = new List<int> { 86, 87 },
                    CommentCount = new List<int> { 88, 89 },
                    Timestamp = DateTime.Now.AddDays(-14)
                },
                new Posting
                {
                    PostId = 16,
                    Avatar = "placeholder_avatar.png",
                    Name = "David Taylor",
                    UserId = 16,
                    Username = "@davidtaylor",
                    PostText = "Dies ist Beispiel-Post 16",
                    Likes = new List<int> { 90, 91 },
                    Dislikes = new List<int> { 92 },
                    CommentCount = new List<int> { 93, 94, 95 },
                    Timestamp = DateTime.Now.AddDays(-15)
                },
                new Posting
                {
                    PostId = 17,
                    Avatar = "placeholder_avatar.png",
                    Name = "Sophie Clark",
                    UserId = 17,
                    Username = "@sophieclark",
                    PostText = "Dies ist Beispiel-Post 17",
                    Likes = new List<int> { 96, 97, 98 },
                    Dislikes = new List<int> { 99, 100 },
                    CommentCount = new List<int> { 101 },
                    Timestamp = DateTime.Now.AddDays(-16)
                },
                new Posting
                {
                    PostId = 18,
                    Avatar = "placeholder_avatar.png",
                    Name = "William Harris",
                    UserId = 18,
                    Username = "@williamharris",
                    PostText = "Dies ist Beispiel-Post 18",
                    Likes = new List<int> { 102, 103 },
                    Dislikes = new List<int> { 104, 105 },
                    CommentCount = new List<int> { 106, 107 },
                    Timestamp = DateTime.Now.AddDays(-17)
                },
                new Posting
                {
                    PostId = 19,
                    Avatar = "placeholder_avatar.png",
                    Name = "Emily Rodriguez",
                    UserId = 19,
                    Username = "@emilyrodriguez",
                    PostText = "Dies ist Beispiel-Post 19",
                    Likes = new List<int> { 108 },
                    Dislikes = new List<int> { 109, 110 },
                    CommentCount = new List<int> { 111, 112 },
                    Timestamp = DateTime.Now.AddDays(-18)
                },
                new Posting
                {
                    PostId = 20,
                    Avatar = "placeholder_avatar.png",
                    Name = "Alexander White",
                    UserId = 20,
                    Username = "@alexanderwhite",
                    PostText = "Dies ist Beispiel-Post 20",
                    Likes = new List<int> { 113, 114 },
                    Dislikes = new List<int> { 115 },
                    CommentCount = new List<int> { 116, 117, 118 },
                    Timestamp = DateTime.Now.AddDays(-19)
                }

            };
            return dummyPosts;
        }
        public static List<DummyPost> Where(List<DummyPost> posts, Func<DummyPost, bool> predicate)
        {
            var filteredPosts = new List<DummyPost>();

            foreach (var post in posts)
            {
                if (predicate(post))
                    filteredPosts.Add(post);
            }

            return filteredPosts;
        }

    }
}
