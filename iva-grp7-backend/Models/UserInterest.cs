namespace iva_grp7_backend.Models;

public class UserInterest
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string Interest { get; set; }
}