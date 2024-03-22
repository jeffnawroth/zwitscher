namespace iva_grp7_backend.Models;

public class Comment : Post
{
    public Comment()
    {
        Comments = new List<Comment>(); // Initializes the Comments property as an empty list
        Date = DateTime.Now; // Initializes the Date property with the current date and time
    }

    public string ParentPostId { get; set; } // Property for storing the ID of the parent post to which this comment belongs
    public Post ParentPost { get; set; } // Property for storing the reference to the parent post entity
}
