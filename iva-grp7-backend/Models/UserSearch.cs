namespace iva_grp7_backend.Models;

public class UserSearch
{
    public string Id { get; set; } // Property for storing the ID of the user
    public string UserName { get; set; } // Property for storing the username of the user
    public string Name { get; set; } // Property for storing the name of the user
    public byte[] Avatar { get; set; } // Property for storing the avatar of the user as a byte array
}
