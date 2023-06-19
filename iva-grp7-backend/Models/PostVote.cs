using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class PostVote
{
    [Key]
    public string PostId { get; set; }
    public Post Post { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public bool IsUpvote { get; set; } // True = Upvote, False = Downvote
}