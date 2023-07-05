namespace iva_grp7_backend.Models;

public class PostEdit
{
    public string Id { get; set; }
    public string? Text { get; set; }
    public List<string>? Files { get; set; }
}