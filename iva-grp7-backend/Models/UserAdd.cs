namespace iva_grp7_backend.Models;

public class UserAdd
{
    public byte[]? Avatar { get; set; }
    public Role Role { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Gender? Gender { get; set; }
    public string Password { get; set; }
    public string? Bio { get; set; }
    public string? BirthDate { get; set; }
    public List<string>? Interests { get; set; }
}