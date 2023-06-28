using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class File
{
    [Key]
    public string Id { get; set; }  = Guid.NewGuid().ToString();
    public byte[] Data { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string PostId { get; set; }
    public Post Post { get; set; }
}
