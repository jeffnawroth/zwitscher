using System.ComponentModel.DataAnnotations.Schema;

namespace iva_grp7_backend.Models;

public class Post
{
    public Post()
    {
        Date = DateTime.Now;
        Edited = false;
        Comments = new List<Comment>();
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    [NotMapped] public string? Avatar { get; set; }

    [NotMapped] public string Name { get; set; }

    [NotMapped] public string Username { get; set; }

    public string? Text { get; set; }
    public List<PostVote>? Votes { get; set; }
    public DateTime Date { get; set; }
    public List<PostFile>? Files { get; set; }
    public bool Edited { get; set; }
    public List<Comment> Comments { get; set; }
}