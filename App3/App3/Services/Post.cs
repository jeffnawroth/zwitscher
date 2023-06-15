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
        public int[] ThumbsUpUserIds { get; set; }
        public int[] ThumbsDownUserIds { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
