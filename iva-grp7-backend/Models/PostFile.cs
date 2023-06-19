using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class PostFile
{
    [Key]
    public string Id { get; set; }  = Guid.NewGuid().ToString();
    public string PostId { get; set; }
    public Post Post { get; set; }
    public byte[] Files { get; set; }
}