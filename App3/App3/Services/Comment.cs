using System;
using System.Collections.Generic;

namespace App3.Services
{
    public class Comment : Posting
    {
        public Comment() 
        {
            Comments = new List<Comment>();
            Date = DateTime.Now;
        }
        public string ParentPostId { get; set; }
        public Posting ParentPost { get; set; }
    }
}