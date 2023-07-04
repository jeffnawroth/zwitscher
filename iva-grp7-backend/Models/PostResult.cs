namespace iva_grp7_backend.Models;

public class PostResult
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public Role UserRole { get; set; }
    public byte[]? Avatar { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string? Text { get; set; }
    public List<string>? UpVotes { get; set; }
    public List<string>? DownVotes { get; set; }
    public DateTime Date { get; set; }
    public List<byte[]>? Files { get; set; }
}