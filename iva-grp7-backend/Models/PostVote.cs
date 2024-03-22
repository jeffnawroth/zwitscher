using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class PostVote
{
    [Key]
    public string PostId { get; set; } // Property for storing the ID of the post being voted on

    public Post Post { get; set; } // Property for storing the reference to the post entity
    public string UserId { get; set; } // Property for storing the ID of the user who cast the vote
    public ApplicationUser User { get; set; } // Property for storing the reference to the user who cast the vote
    public bool IsUpvote { get; set; } // Property indicating whether the vote is an upvote (true) or a downvote (false)
}
