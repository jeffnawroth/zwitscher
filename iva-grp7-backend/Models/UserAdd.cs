namespace iva_grp7_backend.Models;

public class UserAdd
{
    public string? Avatar { get; set; } // Optional property for storing the avatar of the user
    public Role Role { get; set; } // Property for storing the role of the user
    public string Username { get; set; } // Property for storing the username of the user
    public string Name { get; set; } // Property for storing the name of the user
    public string Email { get; set; } // Property for storing the email address of the user
    public Gender? Gender { get; set; } // Optional property for storing the gender of the user
    public string Password { get; set; } // Property for storing the password of the user
    public string? Bio { get; set; } // Optional property for storing the biography of the user
    public string? BirthDate { get; set; } // Optional property for storing the birthdate of the user
    public List<string>? Interests { get; set; } // Optional property for storing the interests of the user
}
