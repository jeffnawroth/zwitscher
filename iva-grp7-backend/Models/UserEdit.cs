using System.ComponentModel.DataAnnotations.Schema;

namespace iva_grp7_backend.Models;

public class UserEdit: User
{
    public IFormFile? Avatar { get; set; }
}