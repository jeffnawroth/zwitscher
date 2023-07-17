namespace iva_grp7_backend.Models;

public class CommentAdd : PostAdd
{
    public string ParentPostId { get; set; } // Property for storing the ID of the parent post to which this comment is being added
}
