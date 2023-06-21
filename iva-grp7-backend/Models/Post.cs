using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace iva_grp7_backend.Models
{
	public class Post
    {
	    public string Id { get; set; } = Guid.NewGuid().ToString();
	    public string UserId { get; set; }
	    public ApplicationUser User { get; set; }
	    
	    [NotMapped]
	    public string? Avatar { get; set; }
	    [NotMapped]
	    public string Name { get; set; }
	    [NotMapped]
	    public string Username { get; set; }
	    public string? Text { get; set; }
	    public List<PostVote>? Votes { get; set; }
	    public DateTime Date { get; set; }
	    public List<PostFile>? Files { get; set; }

	    public Post()
	    {
		    Date = DateTime.UtcNow;
	    }
    }
}

