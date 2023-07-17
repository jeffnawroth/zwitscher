namespace iva_grp7_backend.Models;

public class PostEdit
{
    public string Id { get; set; } // Property for storing the ID of the post being edited
    public string? Text { get; set; } // Optional property for storing the updated text content of the post
    public List<string>? Files { get; set; } // Optional property for storing a list of updated file URLs associated with the post
}
