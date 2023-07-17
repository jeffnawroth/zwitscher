using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class UserFollowing
{
    [Key]
    public string UserId { get; set; } // Property for storing the ID of the user who is following another user

    public ApplicationUser User { get; set; } // Property for storing the reference to the user who is following
    public string FollowingId { get; set; } // Property for storing the ID of the user being followed
    public ApplicationUser Following { get; set; } // Property for storing the reference to the user being followed
}
