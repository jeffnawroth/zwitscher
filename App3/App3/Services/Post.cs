using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Services
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public List<int> ThumbsUpUserIds { get; set; }
        public List<int> ThumbsDownUserIds { get; set; }

        public Post()
        {
            ThumbsUpUserIds = new List<int>();
            ThumbsDownUserIds = new List<int>();
        }

        public void AddThumbsUp(int userId)
        {
            if (!ThumbsUpUserIds.Contains(userId))
                ThumbsUpUserIds.Add(userId);
        }

        public void RemoveThumbsUp(int userId)
        {
            if (ThumbsUpUserIds.Contains(userId))
                ThumbsUpUserIds.Remove(userId);
        }

        public void AddThumbsDown(int userId)
        {
            if (!ThumbsDownUserIds.Contains(userId))
                ThumbsDownUserIds.Add(userId);
        }

        public void RemoveThumbsDown(int userId)
        {
            if (ThumbsDownUserIds.Contains(userId))
                ThumbsDownUserIds.Remove(userId);
        }
    }
}
