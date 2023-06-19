using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class UserFollower
{
    [Key]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string FollowerId { get; set; }
    public ApplicationUser Follower { get; set; }
}