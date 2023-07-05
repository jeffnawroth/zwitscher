using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class UserFollowing
{
    [Key] public string UserId { get; set; }

    public ApplicationUser User { get; set; }
    public string FollowingId { get; set; }
    public ApplicationUser Following { get; set; }
}