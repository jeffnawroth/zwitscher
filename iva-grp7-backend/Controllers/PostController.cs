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
    // Fetch the user by their ID
    var user = await _userManager.FindByIdAsync(postAdd.UserId);

    // If the user does not exist, return a 404 error
    if (user == null) return NotFound("User was not found");

    // Create a new Post object with the given information
    var post = new Post
    {
        UserId = postAdd.UserId,
        Text = postAdd.Text,
        Name = user.Name,
        Username = user.UserName,
        Comments = new List<Comment>()
    };

    // If files have been included in the PostAdd object
    if (postAdd.Files != null)
    {
        post.Files = new List<PostFile>();
        // Loop over all of the files
        foreach (var fileData in postAdd.Files)
        {
            var fileSplit = fileData.Split(',');
            byte[] fileBytes;
            try
            {
                // Try to convert the Base64 string to a byte array
                fileBytes = Convert.FromBase64String(fileSplit[1]);
            }
            catch (FormatException)
            {
                // If the conversion fails, return a 400 error
                return BadRequest("Invalid File format. Please provide a Base64 string.");
            }

            // Create a new PostFile object with the file's data
            var postFile = new PostFile
            {
                Data = fileBytes,
                PostId = post.Id,
                MediaType = fileSplit[0] // Store the media type
            };
            // Add the PostFile object to the Post's Files list
            post.Files.Add(postFile);
        }
    }

    // Add the new Post object to the database context
    _context.Posts.Add(post);
    // Save changes to the database
    await _context.SaveChangesAsync();

    // Create a new PostResult object with the newly created Post's information
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
                new List<string>(),
        Edited = false,
        Comments = new List<CommentResult>()
    };

    // Return the PostResult object in the response, and a 201 status code
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
    // Query all Posts that don't have any Comments linked to them, and include related entities
    var posts = await _context.Posts
        .Where(p => !_context.Comments.Any(c => c.Id == p.Id))
        .Include(p => p.Votes)
        .Include(p => p.Files)
        .Include(p => p.Comments)
        .ToListAsync();

    // Create a new list to store the PostResult objects
    var postResults = new List<PostResult>();

    // Loop over all of the Posts
    foreach (var post in posts)
    {
        // Fetch the User associated with the current Post
        var user = await _userManager.FindByIdAsync(post.UserId);
        // If the User is not null (they exist)
        if (user != null)
        {
            // Create a new PostResult object with the Post and User information
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
                // For UpVotes and DownVotes, select only UserIds who performed these actions
                UpVotes = post.Votes.Where(v => v.IsUpvote).Select(v => v.UserId).ToList(),
                DownVotes = post.Votes.Where(v => !v.IsUpvote).Select(v => v.UserId).ToList(),
                Files = post.Files?.Select(f => $"{f.MediaType},{Convert.ToBase64String(f.Data)}").ToList() ??
                        new List<string>(),
                Edited = post.Edited,
                // For each Comment, create a new CommentResult object and add it to the list
                Comments = post.Comments.Select(c => new CommentResult
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Text = c.Text,
                    Date = c.Date,
                    Edited = c.Edited
                }).ToList()
            };

            // Add the new PostResult object to the list
            postResults.Add(postResult);
        }
    }

    // Return the list of PostResult objects
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
    // Find the post, include the Votes, PostFiles and Comments in the query
    var post = await _context.Posts
        // Load Votes related to the Post
        .Include(p => p.Votes) 
        // Load Files related to the Post
        .Include(p => p.Files)
        // Load Comments related to the Post (level 1)
        .Include(p => p.Comments) 
            // Load Votes related to the Comments (level 1)
            .ThenInclude(c => c.Votes)
        // Load Comments related to the Post again (level 1)
        .Include(p => p.Comments)
            // Load Files related to the Comments (level 1)
            .ThenInclude(c => c.Files)
        // Continue including related data up to level 4 comments
        .Include(p => p.Comments)
            .ThenInclude(c => c.Comments)
                .ThenInclude(cc => cc.Votes)
        .Include(p => p.Comments)
            .ThenInclude(c => c.Comments)
                .ThenInclude(cc => cc.Files)
        .Include(p => p.Comments)
            .ThenInclude(c => c.Comments)
                .ThenInclude(cc => cc.Comments)
                    .ThenInclude(ccc => ccc.Votes)
        .Include(p => p.Comments)
            .ThenInclude(c => c.Comments)
                .ThenInclude(cc => cc.Comments)
                    .ThenInclude(ccc => ccc.Files)
        .Include(p => p.Comments)
            .ThenInclude(c => c.Comments)
                .ThenInclude(cc => cc.Comments)
                    .ThenInclude(ccc => ccc.Comments)
                        .ThenInclude(cccc => cccc.Votes)
        .Include(p => p.Comments)
            .ThenInclude(c => c.Comments)
                .ThenInclude(cc => cc.Comments)
                    .ThenInclude(ccc => ccc.Comments)
                        .ThenInclude(cccc => cccc.Files)
        // Fetch the post that matches the given id
        .SingleOrDefaultAsync(p => p.Id == id);

    // If no matching post was found, return a 404 error
    if (post == null) return NotFound();

    // Fetch the User related to the Post
    var user = await _userManager.FindByIdAsync(post.UserId);
    // If the User exists
    if (user != null)
    {
        // Create a PostResult object and fill it with information from the Post and User
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
                    new List<string>(),
            Edited = post.Edited,
            Comments = new List<CommentResult>()
        };
        // Iterate through each comment and create a CommentResult
        foreach (var comment in post.Comments)
        {
            var commentResult = await CreateCommentResult(comment);
            postResult.Comments.Add(commentResult);
        }

        // Return the created PostResult
        return postResult;
    }

    // If the User doesn't exist, return a 404 error
    return NotFound();
}


    private async Task<CommentResult> CreateCommentResult(Comment comment)
    {
        // Load User information
        var user = await _userManager.FindByIdAsync(comment.UserId);

        // Create a CommentResult object and fill it with information from Comment and User
        var commentResult = new CommentResult
        {
            Id = comment.Id,
            UserId = comment.UserId,
            UserRole = user.Role,
            Name = user.Name,
            Avatar = user.Avatar,
            Username = user.UserName,
            Text = comment.Text,
            Date = comment.Date,
            UpVotes = comment.Votes?.Where(v => v.IsUpvote).Select(v => v.UserId).ToList() ?? new List<string>(),
            DownVotes = comment.Votes?.Where(v => !v.IsUpvote).Select(v => v.UserId).ToList() ?? new List<string>(),
            Files = comment.Files?.Select(f => $"{f.MediaType},{Convert.ToBase64String(f.Data)}").ToList() ??
                    new List<string>(),
            Edited = comment.Edited,
            Comments = new List<CommentResult>()
        };

        // For each nested comment, create a CommentResult and add it to the list of comments for the current commentResult
        foreach (var nestedComment in comment.Comments)
        {
            var nestedCommentResult = await CreateCommentResult(nestedComment);
            commentResult.Comments.Add(nestedCommentResult);
        }

        // Return the created CommentResult
        return commentResult;
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
    // Identify the user
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var user = await _userManager.FindByEmailAsync(userId);
    // Check if the user exists
    if (user == null) return Unauthorized();

    // Find all the users that the current user is following
    var followedUserIds = await _context.UserFollowings
        .Where(f => f.UserId == user.Id)
        .Select(f => f.FollowingId)
        .ToListAsync();

    // Load all posts created by the followed users
    var posts = await _context.Posts
        .Include(p => p.Votes)
        .Include(p => p.Files)
        .Include(p => p.Comments)
        .Where(p => followedUserIds.Contains(p.UserId))
        .Where(p => !_context.Comments.Any(c => c.Id == p.Id))
        .ToListAsync();

    // List to hold the PostResults
    var postResults = new List<PostResult>();

    // For each post, load the user information and votes and set them in a PostResult
    foreach (var post in posts)
    {
        var postUser = await _userManager.FindByIdAsync(post.UserId);
        // Check if the post user exists
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
                        new List<string>(),
                Edited = post.Edited,
                Comments = post.Comments.Select(c => new CommentResult
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Text = c.Text,
                    Date = c.Date,
                    Edited = c.Edited
                }).ToList()
            };
            postResults.Add(postResult);
        }
    }

    // Return the list of PostResults
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
    [HttpGet("user/{username}")]
    public async Task<ActionResult<IEnumerable<PostResult>>> GetPostsByUser(string username)
{
    // Verify if the user exists
    var user = await _userManager.FindByNameAsync(username);
    if (user == null) return NotFound();

    // Load all posts created by the user
    var userPosts = await _context.Posts
        .Include(p => p.Votes)
        .Include(p => p.Files)
        .Where(p => p.UserId == user.Id)
        .Where(p => !_context.Comments.Any(c => c.Id == p.Id))
        .ToListAsync();

    // Load all comments made by the user
    var userComments = await _context.Comments
        .Where(c => c.UserId == user.Id)
        .Include(c => c.ParentPost).ThenInclude(p => p.Votes)
        .Include(c => c.ParentPost).ThenInclude(p => p.Files)
        .ToListAsync();

    var postResults = new List<PostResult>();

    // Set the user information for each post
    foreach (var post in userPosts)
    {
        var postResult = CreatePostResult(user, post);
        postResults.Add(postResult);
    }

    // Add each parent post of the comments and the comment itself
    foreach (var comment in userComments)
    {
        // If the parent post is not already in the list, we add it
        if (!postResults.Any(pr => pr.Id == comment.ParentPost.Id))
        {
            var parentUser = await _userManager.FindByIdAsync(comment.ParentPost.UserId);
            var postResult = CreatePostResult(parentUser, comment.ParentPost);
            postResults.Add(postResult);
        }

        // We add the comment to the comment list of the parent post
        var postResultWithComment = postResults.Single(pr => pr.Id == comment.ParentPost.Id);
        var commentResult = new CommentResult
        {
            Id = comment.Id,
            UserId = comment.UserId,
            Text = comment.Text,
            Date = comment.Date,
            Edited = comment.Edited,
            Avatar = comment.User.Avatar,
            UserRole = comment.User.Role,
            Name = comment.User.Name,
            Username = comment.User.UserName,
            UpVotes = comment.Votes?.Where(v => v.IsUpvote).Select(v => v.UserId).ToList() ?? new List<string>(),
            DownVotes = comment.Votes?.Where(v => !v.IsUpvote).Select(v => v.UserId).ToList() ?? new List<string>(),
            Files = comment.Files?.Select(f => $"{f.MediaType},{Convert.ToBase64String(f.Data)}").ToList() ??
                    new List<string>(),
            ParentPostId = comment.ParentPostId,
            Comments = new List<CommentResult>()
        };
        postResultWithComment.Comments.Add(commentResult);
    }

    return postResults;
}

private PostResult CreatePostResult(ApplicationUser user, Post post)
{
    return new PostResult
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
                new List<string>(),
        Edited = post.Edited,
        Comments = new List<CommentResult>() // We initialize the comment list here, we fill it later
    };
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
        // Find the post with its comments
        var post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
        if (post == null) return NotFound();

        // Delete all comments associated with the post
        _context.Comments.RemoveRange(post.Comments);

        // Then delete the post itself
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return Ok("Post was successfully deleted");
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
    // Find the post with its associated files
    var post = await _context.Posts
        .Include(p => p.Files)
        .FirstOrDefaultAsync(p => p.Id == updatedPost.Id);

    // If the post doesn't exist, return a 'Not Found' status
    if (post == null) return NotFound("Post not found");

    // Update the text and the 'Edited' flag of the post
    post.Text = updatedPost.Text;
    post.Edited = true;

    // Remove all existing files linked to the post
    _context.PostFiles.RemoveRange(post.Files);

    // Add the new files if any
    if (updatedPost.Files != null)
    {
        post.Files = new List<PostFile>();
        foreach (var fileData in updatedPost.Files)
        {
            // Each fileData is a Base64 string with a leading media type
            var fileSplit = fileData.Split(',');
            byte[] fileBytes;
            try
            {
                // Try to convert the Base64 string to byte array
                fileBytes = Convert.FromBase64String(fileSplit[1]);
            }
            catch (FormatException)
            {
                // If the Base64 string is not valid, return a 'Bad Request' status
                return BadRequest("Invalid file format. Please provide a Base64 string.");
            }

            // Create a new PostFile
            var postFile = new PostFile
            {
                Data = fileBytes,
                PostId = post.Id,
                MediaType = fileSplit[0] // Store the media type
            };
            post.Files.Add(postFile);
        }
    }

    try
    {
        // Save the changes to the database
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        // If the post does not exist anymore (deleted by another user), return a 'Not Found' status
        if (!_context.Posts.Any(e => e.Id == updatedPost.Id))
            return NotFound();
        // If another kind of concurrency exception occurred, let the exception propagate
        throw;
    }

    // If the update is successful, return an 'OK' status with a confirmation message
    return Ok("Post updated successfully");
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
        // Fetch the user ID from the current HttpContext
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Retrieve the current user
        var currentUser = await _userManager.FindByEmailAsync(userId);

        // Find the post
        var post = await _context.Posts.FindAsync(postId);

        // If the post does not exist, return a 'Not Found' status
        if (post == null) return NotFound();

        // Find an existing vote by the current user on the post
        var existingVote = await _context.PostVotes
            .SingleOrDefaultAsync(v => v.PostId == postId && v.UserId == currentUser.Id);

        // If an existing vote is found
        if (existingVote != null)
        {
            if (!existingVote.IsUpvote)
            {
                // User had previously downvoted, now changing their vote to upvote
                existingVote.IsUpvote = true;
            }
            else
            {
                // User had previously upvoted, now they are removing their vote
                _context.PostVotes.Remove(existingVote);
            }
        }
        else
        {
            // If no existing vote was found, create a new upvote
            var vote = new PostVote
            {
                PostId = postId,
                UserId = currentUser.Id,
                IsUpvote = true
            };
            _context.PostVotes.Add(vote);
        }

        // Save changes
        await _context.SaveChangesAsync();

        // If successful, return an 'OK' status with a confirmation message
        return Ok("Upvote given to post!");
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
        // Fetch the user ID from the current HttpContext
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Retrieve the current user
        var currentUser = await _userManager.FindByEmailAsync(userId);

        // Find the post
        var post = await _context.Posts.FindAsync(postId);

        // If the post does not exist, return a 'Not Found' status
        if (post == null) return NotFound();

        // Find an existing vote by the current user on the post
        var existingVote = await _context.PostVotes
            .SingleOrDefaultAsync(v => v.PostId == postId && v.UserId == currentUser.Id);

        // If an existing vote is found
        if (existingVote != null)
        {
            if (existingVote.IsUpvote)
            {
                // User had previously upvoted, now changing their vote to downvote
                existingVote.IsUpvote = false;
            }
            else
            {
                // User had previously downvoted, now they are removing their vote
                _context.PostVotes.Remove(existingVote);
            }
        }
        else
        {
            // If no existing vote was found, create a new downvote
            var vote = new PostVote
            {
                PostId = postId,
                UserId = currentUser.Id,
                IsUpvote = false
            };
            _context.PostVotes.Add(vote);
        }

        // Save changes
        await _context.SaveChangesAsync();

        // If successful, return an 'OK' status with a confirmation message
        return Ok("Downvote given to post!");
    }


    /// <summary>
    ///     Creates a new comment.
    /// </summary>
    /// <param name="comment">The comment to create.</param>
    /// <returns>The created comment.</returns>
    /// <response code="201">Returns the created comment.</response>
    /// <response code="400">If the comment data is invalid.</response>
    /// <response code="500">If an exception occurs while creating the comment.</response>
    [HttpPost("Comment")]
    [ProducesResponseType(201, Type = typeof(CommentResult))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PostResult>> CreateComment(CommentAdd commentAdd)
{
    // Find the user
    var user = await _userManager.FindByIdAsync(commentAdd.UserId);

    // If the user does not exist, return a 'Not Found' status
    if (user == null) return NotFound("User not found");

    // Find the parent post and include its comments
    var parentPost = await _context.Posts.Include(p => p.Comments)
        .SingleOrDefaultAsync(p => p.Id == commentAdd.ParentPostId);

    // If the parent post does not exist, return a 'Not Found' status
    if (parentPost == null) return NotFound("Parent post not found");

    // Create a new comment
    var comment = new Comment
    {
        UserId = user.Id,
        User = user,
        Text = commentAdd.Text,
        Name = user.Name, // Add the user's name
        Username = user.UserName, // Add the user's username
        ParentPostId = commentAdd.ParentPostId
    };

    // If the comment contains files
    if (commentAdd.Files != null)
    {
        comment.Files = new List<PostFile>();
        foreach (var fileData in commentAdd.Files)
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

            // Create a new post file
            var postFile = new PostFile
            {
                Data = fileBytes,
                PostId = comment.Id,
                MediaType = fileSplit[0] // Store media type
            };
            comment.Files.Add(postFile);
        }
    }

    // Add the comment to the post
    parentPost.Comments.Add(comment);
    await _context.SaveChangesAsync();

    // Return the comment
    var commentResult = new CommentResult
    {
        Id = comment.Id,
        UserId = comment.UserId,
        UserRole = user.Role,
        Avatar = user.Avatar,
        Name = user.Name,
        Username = user.UserName,
        Text = comment.Text,
        Date = comment.Date,
        Files = comment.Files?.Select(f => $"{f.MediaType},{Convert.ToBase64String(f.Data)}").ToList() ??
                new List<string>(),
        Edited = false,
        ParentPostId = commentAdd.ParentPostId,
        Comments = new List<CommentResult>()
    };

    return CreatedAtAction("GetComment", new {id = comment.Id}, commentResult);
}



    /// <summary>
    ///     Gets a comment by its ID.
    /// </summary>
    /// <param name="id">The ID of the comment.</param>
    /// <returns>The comment with the specified ID.</returns>
    /// <response code="200">Returns the comment.</response>
    /// <response code="404">If the comment is not found.</response>
    /// <response code="500">If an exception occurs while retrieving the comment.</response>
    [ProducesResponseType(200, Type = typeof(CommentResult))]
    [HttpGet("{id}/comment")]
    public async Task<ActionResult<CommentResult>> GetComment(string id)
    {
        // Find the comment, include the Votes, PostFiles and nested Comments in the query
        var comment = await _context.Comments
            .Include(p => p.Votes)
            .Include(p => p.Files)
            .Include(p => p.Comments) // Include nested comments
            .SingleOrDefaultAsync(p => p.Id == id);

        // If the comment does not exist, return a 'Not Found' status
        if (comment == null) return NotFound();

        // Create the CommentResult recursively
        var commentResult = await CreateCommentResult(comment);

        return commentResult;
    }

}