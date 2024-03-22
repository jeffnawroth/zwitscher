using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class RefreshToken
{
    [Key]
    public int Id { get; set; } // Property for storing the unique identifier of the refresh token

    public string UserId { get; set; } // Property for storing the ID of the user associated with the refresh token
    public string Token { get; set; } // Property for storing the refresh token value
    public string JwtId { get; set; } // Property for storing the JWT ID associated with the refresh token
    public bool IsUsed { get; set; } // Property indicating whether the refresh token has been used
    public bool IsRevoked { get; set; } // Property indicating whether the refresh token has been revoked
    public DateTime AddedDate { get; set; } // Property for storing the date and time when the refresh token was added
    public DateTime ExpiryDate { get; set; } // Property for storing the expiry date and time of the refresh token
}
