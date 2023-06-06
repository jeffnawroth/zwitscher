using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace iva_grp7_backend.Controllers;
    

    /// <summary>
    /// A controller for managing posts.
    /// </summary>
    [Route("api/[controller]")] // api/authentication
    [ApiController]

    public class PostController: ControllerBase
    {
        private readonly ApiDbContext _context;

        public PostController(ApiDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Gets a list of all posts.
        /// </summary>
        /// <returns>A list of all posts.</returns>
        /// <response code="200">Returns the list of posts.</response>
        /// <response code="500">If an exception occurs while retrieving the posts.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Post>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return Ok(posts);
        }
        
        /// <summary>
        /// Gets a post by its ID.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        /// <returns>The post with the specified ID.</returns>
        /// <response code="200">Returns the post.</response>
        /// <response code="404">If the post is not found.</response>
        /// <response code="500">If an exception occurs while retrieving the post.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Post))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPostById(string id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            
            return Ok(post);
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="post">The post to create.</param>
        /// <returns>The created post.</returns>
        /// <response code="201">Returns the created post.</response>
        /// <response code="400">If the post data is invalid.</response>
        /// <response code="500">If an exception occurs while creating the post.</response>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Post))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        /// <summary>
        /// Returns the posts of users that are being followed.
        /// </summary>
        /// <returns>The posts of followed users.</returns>
        /// <response code="200">Returns the posts.</response>
        /// <response code="500">If an exception occurs while retrieving the posts.</response>
        [HttpPost("followedUsers")]
        public async Task<IActionResult> getFollowedUsersPosts()
        {
            // Get the current user
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get the list of users that the current user is following
            var followingUsers = _context.Following
                .Where(f => f.FollowingUserId == currentUserId)
                .Select(f => f.FollowingUserId)
                .ToList();

            // Get the posts from the following users
            var followingPosts = await _context.Posts
                .Where(p => followingUsers.Contains(p.UserId))
                .ToListAsync();

            return Ok(followingPosts);
        }

        /// <summary>
        /// Returns the posts of a specified user.
        /// </summary>
        /// <param name="userID">The userID to find the user.</param>
        /// <returns>The posts of the specified user.</returns>
        /// <response code="200">Returns the posts.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If an exception occurs while retrieving the posts.</response>
        [HttpPost("{userID}")]
        public async Task<IActionResult> getPostsForUser(string userID)
        {
            var postsUser = await _context.Posts
                .Where(p => p.UserId == userID)
                .ToListAsync();
            if(postsUser == null)
            {
                return NotFound();
            }
            return Ok(postsUser);
        }
     

}