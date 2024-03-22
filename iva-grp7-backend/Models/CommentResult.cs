namespace iva_grp7_backend.Models;

public class CommentResult : PostResult
{
    public string ParentPostId { get; set; } // Property for storing the ID of the parent post to which this comment belongs
    public List<CommentResult> Comments { get; set; } // Property for storing the list of child comments associated with this comment
}
