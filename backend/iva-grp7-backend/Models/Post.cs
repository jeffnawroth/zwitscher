using System.ComponentModel.DataAnnotations.Schema;

namespace iva_grp7_backend.Models;

public class Post
{
    public Post()
    {
        Date = DateTime.Now; // Initializes the Date property with the current date and time
        Edited = false; // Initializes the Edited property as false
        Comments = new List<Comment>(); // Initializes the Comments property as an empty list
    }

    public string Id { get; set; } = Guid.NewGuid().ToString(); // Property for storing the unique identifier of the post
    public string UserId { get; set; } // Property for storing the ID of the user who created the post
    public ApplicationUser User { get; set; } // Property for storing the reference to the user who created the post

    [NotMapped]
    public string? Avatar { get; set; } // Property for storing the avatar of the user (not mapped to the database)

    [NotMapped]
    public string Name { get; set; } // Property for storing the name of the user (not mapped to the database)

    [NotMapped]
    public string Username { get; set; } // Property for storing the username of the user (not mapped to the database)

    public string? Text { get; set; } // Property for storing the text content of the post
    public List<PostVote>? Votes { get; set; } // Collection of PostVote entities representing the votes on the post
    public DateTime Date { get; set; } // Property for storing the date and time when the post was created
    public List<PostFile>? Files { get; set; } // Collection of PostFile entities representing the files associated with the post
    public bool Edited { get; set; } // Property indicating whether the post has been edited
    public List<Comment> Comments { get; set; } // Collection of Comment entities representing the comments on the post
}
