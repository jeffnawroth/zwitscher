namespace iva_grp7_backend.Models;

public class AuthResult : User
{
    public string RefreshToken { get; set; }
    public string Token { get; set; }
}