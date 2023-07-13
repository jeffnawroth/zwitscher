namespace iva_grp7_backend.Models;

public class Comment : Post
{
    public Comment()
    {
        Comments = new List<Comment>();
        Date = DateTime.Now;
    }

    public string ParentPostId { get; set; }
    public Post ParentPost { get; set; }
}