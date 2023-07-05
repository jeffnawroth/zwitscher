using System.Security.Claims;
using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iva_grp7_backend.Controllers;

/// <summary>
///     A controller for managing posts.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PostController(ApiDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    ///     Creates a new post.
    /// </summary>
    /// <param name="post">The post to create.</param>
    /// <returns>The created post.</returns>
    /// <response code="201">Returns the created post.</response>
    /// <response code="400">If the post data is invalid.</response>
    /// <response code="500">If an exception occurs while creating the post.</response>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(PostResult))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PostResult>> CreatePost(PostAdd postAdd)
    {
        var user = await _userManager.FindByIdAsync(postAdd.UserId);

        if (user == null) return NotFound("User wurde nicht gefunden");

        var post = new Post
        {
            UserId = postAdd.UserId,
            Text = postAdd.Text,
            Name = user.Name,
            Username = user.UserName
        };

        if (postAdd.Files != null)
        {
            post.Files = new List<PostFile>();
            foreach (var fileData in postAdd.Files)
            {
                var fileSplit = fileData.Split(',');
                byte[] fileBytes;
                try
                {
                    fileBytes = Convert.FromBase64String(fileSplit[1]);
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid File format. Please provide a Base64 string.");
                }

                var postFile = new PostFile
                {
                    Data = fileBytes,
                    PostId = post.Id,
                    MediaType = fileSplit[0] // Medientyp speichern
                };
                post.Files.Add(postFile);
            }
        }

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var postResult = new PostResult
        {
            Id = post.Id,
            UserId = post.UserId,
            UserRole = user.Role,
            Avatar = user.Avatar,
            Name = user.Name,
            Username = user.UserName,
            Text = post.Text,
            Date = post.Date,
            Files = post.Files?.Select(f => $"{f.MediaType},{Convert.ToBase64String(f.Data)}").ToList() ??
                    new List<string>()
        };

        return CreatedAtAction("GetPost", new {id = post.Id}, postResult);
    }


    /// <summary>
    ///     Gets all posts.
    /// </summary>
    /// <returns>A list of all posts.</returns>
    /// <response code="200">Returns the list of posts.</response>
    /// <response code="500">If an exception occurs while retrieving the posts.</response>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<PostResult>))]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IEnumerable<PostResult>>> GetAllPublicPosts()
    {
        var posts = await _context.Posts
            .Include(p => p.Votes)
            .Include(p => p.Files)
            .ToListAsync();

        var postResults = new List<PostResult>();

        foreach (var post in posts)
        {
            var user = await _userManager.FindByIdAsync(post.UserId);
            if (user != null)
            {
                var postResult = new PostResult
                {
                    Id = post.Id,
                    UserId = post.UserId,
                    UserRole = user.Role,
                    Avatar = user.Avatar,
                    Name = user.Name,
                    Username = user.UserName,
                    Text = post.Text,
                    Date = post.Date,
                    UpVotes = post.Votes.Where(v => v.IsUpvote).Select(v => v.UserId).ToList(),
                    DownVotes = post.Votes.Where(v => !v.IsUpvote).Select(v => v.UserId).ToList(),
                    Files = post.Files?.Select(f => $"{f.MediaType},{Convert.ToBase64String(f.Data)}").ToList() ??
                            new List<string>()
                };

                postResults.Add(postResult);
            }
        }

        return postResults;
    }


    /// <summary>
    ///     Gets a post by its ID.
    /// </summary>
    /// <param name="id">The ID of the post.</param>
    /// <returns>The post with the specified ID.</returns>
    /// <response code="200">Returns the post.</response>
    /// <response code="404">If the post is not found.</response>
    /// <response code="500">If an exception occurs while retrieving the post.</response>
    [ProducesResponseType(200, Type = typeof(PostResult))]
    [HttpGet("{id}")]
    public async Task<ActionResult<PostResult>> GetPost(string id)
    {
        // Find the post, include the Votes and PostFiles in the query
        var post = await _context.Posts.Include(p => p.Votes).Include(p => p.Files)
            .SingleOrDefaultAsync(p => p.Id == id);

        if (post == null) return NotFound();

        // User Informationen laden
        var user = await _userManager.FindByIdAsync(post.UserId);
        if (user != null)
        {
            // Erstellen Sie ein PostResult-Objekt und füllen Sie die Informationen aus Post und User aus
            var postResult = new PostResult
            {
                Id = post.Id,
                UserId = post.UserId,
                UserRole = user.Role,
                Name = user.Name,
                Avatar = user.Avatar,
                Username = user.UserName,
                Text = post.Text,
                Date = post.Date,
                UpVotes = post.Votes.Where(v => v.IsUpvote).Select(v => v.UserId).ToList(),
                DownVotes = post.Votes.Where(v => !v.IsUpvote).Select(v => v.UserId).ToList(),
                Files = post.Files?.Select(f => $"{f.MediaType},{Convert.ToBase64String(f.Data)}").ToList() ??
                        new List<string>()
            };

            return postResult;
        }

        return NotFound();
    }


    /// <summary>
    ///     Gets all posts from following users.
    /// </summary>
    /// <returns>A list of all posts from following users.</returns>
    /// <response code="200">Returns the list of all posts from following users.</response>
    /// <response code="500">If an exception occurs while retrieving the posts.</response>
    [ProducesResponseType(200, Type = typeof(List<PostResult>))]
    [HttpGet("followingPosts")]
    public async Task<ActionResult<IEnumerable<PostResult>>> GetPostsFromFollowedUsers()
    {
        // Benutzer ermitteln
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByEmailAsync(userId);
        if (user == null) return Unauthorized();

        // Finden Sie alle Benutzer, denen der aktuelle Benutzer folgt
        var followedUserIds = await _context.UserFollowings
            .Where(f => f.UserId == user.Id)
            .Select(f => f.FollowingId)
            .ToListAsync();

        // Alle Posts laden, die von den gefolgten Benutzern erstellt wurden
        var posts = await _context.Posts
            .Include(p => p.Votes)
            .Include(p => p.Files)
            .Where(p => followedUserIds.Contains(p.UserId))
            .ToListAsync();

        // Liste für die PostResults
        var postResults = new List<PostResult>();

        // Für jeden Post die User-Informationen und die Votes laden und setzen
        foreach (var post in posts)
        {
            var postUser = await _userManager.FindByIdAsync(post.UserId);
            if (postUser != null)
            {
                var postResult = new PostResult
                {
                    Id = post.Id,
                    UserId = post.UserId,
                    UserRole = postUser.Role,
                    Name = postUser.Name,
                    Avatar = postUser.Avatar,
                    Username = postUser.UserName,
                    Text = post.Text,
                    Date = post.Date,
                    UpVotes = post.Votes.Where(v => v.IsUpvote).Select(v => v.UserId).ToList(),
                    DownVotes = post.Votes.Where(v => !v.IsUpvote).Select(v => v.UserId).ToList(),
                    Files = post.Files?.Select(f => $"{f.MediaType},{Convert.ToBase64String(f.Data)}").ToList() ??
                            new List<string>()
                };
                postResults.Add(postResult);
            }
        }

        return postResults;
    }


    /// <summary>
    ///     Gets all posts from a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of all posts from the user.</returns>
    /// <response code="200">Returns the list of posts.</response>
    /// <response code="404">If the user is not found.</response>
    /// <response code="500">If an exception occurs while retrieving the posts.</response>
    [ProducesResponseType(200, Type = typeof(List<PostResult>))]
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<PostResult>>> GetPostsByUser(string userId)
    {
        // Überprüfen, ob der Benutzer existiert
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        // Alle Posts des Benutzers laden, including the Votes and Files
        var posts = await _context.Posts.Include(p => p.Votes).Include(p => p.Files)
            .Where(p => p.UserId == userId)
            .ToListAsync();

        var postResults = new List<PostResult>();

        // Benutzerinformationen für jeden Post setzen
        foreach (var post in posts)
        {
            var postResult = new PostResult
            {
                Id = post.Id,
                UserId = post.UserId,
                UserRole = user.Role,
                Name = user.Name,
                Avatar = user.Avatar,
                Username = user.UserName,
                Text = post.Text,
                Date = post.Date,
                UpVotes = post.Votes.Where(v => v.IsUpvote).Select(v => v.UserId).ToList(),
                DownVotes = post.Votes.Where(v => !v.IsUpvote).Select(v => v.UserId).ToList(),
                Files = post.Files?.Select(f => $"{f.MediaType},{Convert.ToBase64String(f.Data)}").ToList() ??
                        new List<string>()
            };
            postResults.Add(postResult);
        }

        return postResults;
    }


    /// <summary>
    ///     Deletes a post.
    /// </summary>
    /// <param name="id">The ID of the post.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If the post is successfully deleted.</response>
    /// <response code="404">If the post is not found.</response>
    /// <response code="500">If an exception occurs while deleting the post.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(string id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound();

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return Ok("Post wurde erfolgreich gelöscht");
    }

    /// <summary>
    ///     Updates a post.
    /// </summary>
    /// <param name="updatedPost">The updated post data.</param>
    /// <returns>The updated post.</returns>
    /// <response code="200">If the post is successfully updated.</response>
    /// <response code="404">If the post is not found.</response>
    /// <response code="500">If an exception occurs while updating the post.</response>
    [HttpPut]
    public async Task<IActionResult> UpdatePost([FromBody] PostEdit updatedPost)
    {
        var post = await _context.Posts
            .Include(p => p.Files)
            .FirstOrDefaultAsync(p => p.Id == updatedPost.Id);

        if (post == null) return NotFound("Post wurde nicht gefunden");

        post.Text = updatedPost.Text;

        // Remove all existing files
        _context.PostFiles.RemoveRange(post.Files);

        // Add new files
        if (updatedPost.Files != null)
        {
            post.Files = new List<PostFile>();
            foreach (var fileData in updatedPost.Files)
            {
                var fileSplit = fileData.Split(',');
                byte[] fileBytes;
                try
                {
                    fileBytes = Convert.FromBase64String(fileSplit[1]);
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid File format. Please provide a Base64 string.");
                }

                var postFile = new PostFile
                {
                    Data = fileBytes,
                    PostId = post.Id,
                    MediaType = fileSplit[0] // Medientyp speichern
                };
                post.Files.Add(postFile);
            }
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Posts.Any(e => e.Id == updatedPost.Id))
                return NotFound();
            throw;
        }

        return Ok("Post wurde aktualisiert");
    }


    /// <summary>
    ///     Upvotes a post.
    /// </summary>
    /// <param name="postId">The id of the post to upvote.</param>
    /// <returns>A response indicating the result of the upvote action.</returns>
    /// <response code="200">
    ///     If the post is successfully upvoted, or if the user's existing downvote is changed to an upvote,
    ///     or if the user's existing upvote is removed.
    /// </response>
    /// <response code="404">If the post is not found.</response>
    /// <response code="500">If an exception occurs while processing the upvote.</response>
    [HttpPost("{postId}/upvote")]
    public async Task<IActionResult> UpvotePost(string postId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = await _userManager.FindByEmailAsync(userId);

        var post = await _context.Posts.FindAsync(postId);

        if (post == null) return NotFound();

        var existingVote = await _context.PostVotes
            .SingleOrDefaultAsync(v => v.PostId == postId && v.UserId == currentUser.Id);

        if (existingVote != null)
        {
            if (!existingVote.IsUpvote)
                // Benutzer hat bereits Downvote abgegeben, jetzt ändert er seine Meinung
                existingVote.IsUpvote = true;
            else
                // Benutzer hat bereits Upvote abgegeben, entfernt jetzt seine Stimme
                _context.PostVotes.Remove(existingVote);
        }
        else
        {
            // Erstellen Sie eine neue Abstimmung
            var vote = new PostVote
            {
                PostId = postId,
                UserId = currentUser.Id,
                IsUpvote = true
            };
            _context.PostVotes.Add(vote);
        }

        await _context.SaveChangesAsync();

        return Ok("Post wurde ein UpVote gegeben!");
    }

    /// <summary>
    ///     Downvotes a post.
    /// </summary>
    /// <param name="postId">The id of the post to downvote.</param>
    /// <returns>A response indicating the result of the downvote action.</returns>
    /// <response code="200">
    ///     If the post is successfully downvoted, or if the user's existing upvote is changed to a downvote,
    ///     or if the user's existing downvote is removed.
    /// </response>
    /// <response code="404">If the post is not found.</response>
    /// <response code="500">If an exception occurs while processing the downvote.</response>
    [HttpPost("{postId}/downvote")]
    public async Task<IActionResult> DownvotePost(string postId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = await _userManager.FindByEmailAsync(userId);

        var post = await _context.Posts.FindAsync(postId);

        if (post == null) return NotFound();

        var existingVote = await _context.PostVotes
            .SingleOrDefaultAsync(v => v.PostId == postId && v.UserId == currentUser.Id);

        if (existingVote != null)
        {
            if (existingVote.IsUpvote)
                // Benutzer hat bereits Upvote abgegeben, jetzt ändert er seine Meinung
                existingVote.IsUpvote = false;
            else
                // Benutzer hat bereits Downvote abgegeben, entfernt jetzt seine Stimme
                _context.PostVotes.Remove(existingVote);
        }
        else
        {
            // Erstellen Sie eine neue Abstimmung
            var vote = new PostVote
            {
                PostId = postId,
                UserId = currentUser.Id,
                IsUpvote = false
            };
            _context.PostVotes.Add(vote);
        }

        await _context.SaveChangesAsync();

        return Ok("Post wurde ein DownVote gegeben!");
    }
}