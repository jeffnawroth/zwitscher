using Microsoft.AspNetCore.Identity;

namespace iva_grp7_backend.Models;

public class ApplicationUser : IdentityUser
{
    private string _email; // Private backing field for the Email property

    public ApplicationUser()
    {
        CreatedAt = DateTime.UtcNow; // Initializes the CreatedAt property with the current UTC date and time
    }

    public byte[]? Avatar { get; set; } // Property for storing the user's avatar as a byte array
    public Role Role { get; set; } // Property for storing the user's role
    public string Name { get; set; } // Property for storing the user's name
    public Gender? Gender { get; set; } // Property for storing the user's gender (nullable)
    public string? BirthDate { get; set; } // Property for storing the user's birth date as a string (nullable)
    public List<UserFollower> Followers { get; set; } // Collection of UserFollower entities representing the users who follow this user
    public List<UserFollowing> Following { get; set; } // Collection of UserFollowing entities representing the users whom this user follows
    public DateTime CreatedAt { get; set; } // Property for storing the date and time when the user was created
    public string? Bio { get; set; } // Property for storing the user's biography (nullable)
    public List<UserInterest>? Interests { get; set; } // Collection of UserInterest entities representing the user's interests (nullable)
    public bool Locked { get; set; } // Property indicating whether the user is locked or not

    public override string Email
    {
        get => _email; // Custom getter for the Email property
        set
        {
            _email = value; // Sets the private backing field
            NormalizedEmail = _email?.ToUpperInvariant().Normalize(); // Converts and stores the normalized email value
        }
    }
}
