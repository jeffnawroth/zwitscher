using System;
using System.Collections.Generic;

namespace App3.Services
{
    public static class DummyPost
    {
        private static List<Post> dummyPostings;

        static DummyPost()
        {
            dummyPostings = new List<Post>();
            InitializeDummyPostings();
        }

        private static void InitializeDummyPostings()
        {
            // Dummy-Posting 1
            Post dummyPost1 = new Post
            {
                PostId = 1,
                UserId = 1,
                UserAvatar = "avatar_1.jpg",
                ThumbsUpUserIds = new List<int> { 2, 3, 4 },
                ThumbsDownUserIds = new List<int> { 5 },
                Timestamp = DateTime.Now.AddDays(-1)
            };

            // Dummy-Posting 2
            Post dummyPost2 = new Post
            {
                PostId = 2,
                UserId = 2,
                UserAvatar = "avatar_2.jpg",
                ThumbsUpUserIds = new List<int> { 1, 3 },
                ThumbsDownUserIds = new List<int> { 4, 5 },
                Timestamp = DateTime.Now.AddDays(-2)
            };

            // Dummy-Posting 3
            Post dummyPost3 = new Post
            {
                PostId = 3,
                UserId = 3,
                UserAvatar = "avatar_3.jpg",
                ThumbsUpUserIds = new List<int> { 1, 2, 4 },
                ThumbsDownUserIds = new List<int> { 5 },
                Timestamp = DateTime.Now.AddDays(-3)
            };

            // Füge die Dummy-Postings zur Liste hinzu
            dummyPostings.Add(dummyPost1);
            dummyPostings.Add(dummyPost2);
            dummyPostings.Add(dummyPost3);
        }

        public static List<Post> GetDummyPostings()
        {
            return dummyPostings;
        }
    }
}
