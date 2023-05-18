using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Controllers;

public class TokenRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    public string RefreshToken { get; set; }
}