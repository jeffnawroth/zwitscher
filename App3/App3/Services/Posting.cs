using System;
using System.Collections.Generic;

namespace App3
{
    public class Posting
    {
        public int PostId { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PostText { get; set; }
        public List<int> Likes { get; set; }
        public List<int> Dislikes { get; set; }
        public List<int> CommentCount { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
