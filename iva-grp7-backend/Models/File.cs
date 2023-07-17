using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

public class File
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString(); // Property for storing the unique identifier of the file

    public byte[] Data { get; set; } // Property for storing the file data as a byte array
    public string UserId { get; set; } // Property for storing the ID of the user who uploaded the file
    public ApplicationUser User { get; set; } // Property for storing the reference to the user who uploaded the file
    public string PostId { get; set; } // Property for storing the ID of the post to which the file is associated
    public Post Post { get; set; } // Property for storing the reference to the post to which the file is associated
}
