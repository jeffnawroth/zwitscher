namespace iva_grp7_backend.Models;

public class UserInterest
{
    public string UserId { get; set; } // Property for storing the ID of the user
    public ApplicationUser User { get; set; } // Property for storing the reference to the user
    public string Interest { get; set; } // Property for storing the interest of the user
}
