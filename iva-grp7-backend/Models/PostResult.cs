namespace iva_grp7_backend.Models;

public class PostResult
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // Property for storing the unique identifier of the post
    public string UserId { get; set; } // Property for storing the ID of the user who created the post
    public Role UserRole { get; set; } // Property for storing the role of the user who created the post
    public byte[]? Avatar { get; set; } // Property for storing the avatar of the user (nullable)
    public string Name { get; set; } // Property for storing the name of the user
    public string Username { get; set; } // Property for storing the username of the user
    public string? Text { get; set; } // Property for storing the text content of the post (nullable)
    public List<string>? UpVotes { get; set; } // Optional property for storing the IDs of users who upvoted the post
    public List<string>? DownVotes { get; set; } // Optional property for storing the IDs of users who downvoted the post
    public DateTime Date { get; set; } // Property for storing the date and time when the post was created
    public List<string> Files { get; set; } // Property for storing the URLs of files associated with the post
    public bool Edited { get; set; } // Property indicating whether the post has been edited
    public List<CommentResult> Comments { get; set; } // Property for storing the list of comments associated with the post
}
