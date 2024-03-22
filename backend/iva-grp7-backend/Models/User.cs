using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class User
{
    public User()
    {
        CreatedAt = DateTime.Now; // Initializes the CreatedAt property with the current date and time
    }

    [Key]
    public string Id { get; set; } // Property for storing the unique identifier of the user

    public byte[]? Avatar { get; set; } // Property for storing the avatar of the user (nullable)
    public Role Role { get; set; } // Property for storing the role of the user
    public string Username { get; set; } // Property for storing the username of the user
    public string Name { get; set; } // Property for storing the name of the user
    public string Email { get; set; } // Property for storing the email address of the user
    public Gender? Gender { get; set; } // Property for storing the gender of the user (nullable)
    public string? BirthDate { get; set; } // Property for storing the birthdate of the user (nullable)
    public List<string> Followers { get; set; } // Property for storing the IDs of users who follow this user
    public List<string> Following { get; set; } // Property for storing the IDs of users whom this user follows
    public DateTime CreatedAt { get; set; } // Property for storing the date and time when the user was created
    public string? Bio { get; set; } // Property for storing the biography of the user (nullable)
    public List<string>? Interests { get; set; } // Property for storing the interests of the user (nullable)
    public bool Locked { get; set; } // Property indicating whether the user account is locked
}

public enum Role
{
    Admin = 0, // Role value for an admin user
    Moderator = 1, // Role value for a moderator user
    User = 2 // Role value for a regular user
}

public enum Gender
{
    Male = 0, // Gender value for male
    Female = 1, // Gender value for female
    Diverse = 2 // Gender value for diverse
}

public class UserLight
{
    public string Id { get; set; } // Property for storing the ID of the user
    public byte[]? Avatar { get; set; } // Property for storing the avatar of the user (nullable)
    public string Username { get; set; } // Property for storing the username of the user
    public string Name { get; set; } // Property for storing the name of the user
}
