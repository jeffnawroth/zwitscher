using App3.Services;
using System;
using System.Collections.Generic;

namespace App3
{
    public class Posting
    {
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public List<string> UpVotes { get; set; }
        public List<string> DownVotes { get; set; }
        public List<int> CommentCount { get; set; }
        public DateTime Date { get; set; }
        public List<Comment> Comments { get; set; }
    }

}
