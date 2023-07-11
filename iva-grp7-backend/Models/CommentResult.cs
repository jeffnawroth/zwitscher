namespace iva_grp7_backend.Models;

public class CommentResult: PostResult
{
    public string ParentPostId { get; set; }
    public string ParentCommentId { get; set; }
    public List<CommentResult> Comments { get; set; }
}