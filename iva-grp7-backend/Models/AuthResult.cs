namespace iva_grp7_backend.Models;

public class AuthResult : User
{
    public string RefreshToken { get; set; } // Property for storing the refresh token associated with the authentication result
    public string Token { get; set; } // Property for storing the access token associated with the authentication result
    public string Password { get; set; } // Property for storing the password associated with the authentication result
}
