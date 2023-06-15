using System;
using System.Collections.Generic;
using System.Text;


namespace App3
{
    public class Posting
    {
        public string UserID { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public Posting(string userID, string text)
        {
            UserID = userID;
            Text = text;
            Timestamp = DateTime.Now;
        }
    }
}
