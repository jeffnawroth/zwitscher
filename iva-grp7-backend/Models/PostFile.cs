namespace iva_grp7_backend.Models;

public class PostFile
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // Property for storing the unique identifier of the file
    public byte[] Data { get; set; } // Property for storing the file data as a byte array

    public string MediaType { get; set; } // Property for storing the media type of the file (e.g., image/jpeg, video/mp4)
    public string PostId { get; set; } // Property for storing the ID of the post to which the file is associated
    public Post Post { get; set; } // Property for storing the reference to the post to which the file is associated
}
