namespace iva_grp7_backend.Models;

public class PostAdd
{
    public string UserId { get; set; } // Property for storing the ID of the user creating the post
    public string Text { get; set; } // Property for storing the text content of the post
    public List<string>? Files { get; set; } // Optional property for storing a list of file URLs associated with the post
}
