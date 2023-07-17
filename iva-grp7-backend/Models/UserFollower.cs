using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class UserFollower
{
    [Key]
    public string UserId { get; set; } // Property for storing the ID of the user being followed

    public ApplicationUser User { get; set; } // Property for storing the reference to the user being followed
    public string FollowerId { get; set; } // Property for storing the ID of the follower user
    public ApplicationUser Follower { get; set; } // Property for storing the reference to the follower user
}
