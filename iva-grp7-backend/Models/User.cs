using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class User
{
    public User()
    {
        CreatedAt = DateTime.UtcNow;
    }

    [Key] public string Id { get; set; }

    public byte[]? Avatar { get; set; }
    public Role Role { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Gender? Gender { get; set; }
    public string? BirthDate { get; set; }
    public List<string> Followers { get; set; }
    public List<string> Following { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Bio { get; set; }
    public List<string>? Interests { get; set; }
    public bool Locked { get; set; }
}

public enum Role
{
    Admin = 0,

    Moderator = 1,

    User = 2
}

public enum Gender
{
    Male = 0,
    Female = 1,
    Diverse = 2
}

public class Follower
{
    public int Id { get; set; }
    public string FollowerUserId { get; set; } // Benutzer-ID des Followers
    public ApplicationUser User { get; set; } // Navigationseigenschaft zum Benutzer
}

public class Following
{
    public int? Id { get; set; }
    public string? FollowingUserId { get; set; } // Benutzer-ID des Following
    public ApplicationUser? User { get; set; } // Navigationseigenschaft zum Benutzer
}

public class UserLight
{
    public string Id { get; set; }
    public byte[]? Avatar { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
}