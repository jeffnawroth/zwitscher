using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace iva_grp7_backend.Controllers
{
    /// <summary>
    /// A controller for managing the posts.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly UserManager<Post> _postManager;
        private readonly ApiDbContext _context;

        /// <summary>
        /// Creates a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="postManager">The user manager to use for managing posts.</param>
        public PostsController(UserManager<Post> postManager, ApiDbContext context)
        {
            _postManager = postManager;
            _context = context;
        }

        /// <summary>
        /// Gets a list of all posts.
        /// </summary>
        /// <returns>A list of all posts.</returns>
        /// <response code="200">Returns the list of posts.</response>
        /// <response code="500">If an exception occurs while retrieving the posts.</response>
        [Route("allPosts")]
        [HttpGet] 
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _postManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserId,
                    u.Avatar,
                    u.Firstname,
                    u.LastName,
                    u.UserName,
                    u.Text,
                    u.UpVotes,
                    u.DownVotes,
                    u.CreatedAt,
                    u.DislikedByUserId,
                    u.LikedByUserId,
                    DislikedByUser = new {},
                    LikedByUser = new {}
                }).ToListAsync();
            return Ok(posts);
        }

        /// <summary>
        /// Gets a post with the specified userID.
        /// </summary>
        /// <param name="userID">The ID of the user of whom to get the posts from.</param>
        /// <returns>The posts from the specified ID, or a 404 Not Found error if no post exist yet.</returns>
        /// <response code="200">Returns the posts from the specified ID.</response>
        /// <response code="404">If they are no posts from the specified ID.</response>
        /// <response code="500">If an exception occurs while retrieving the posts.</response>
        [Route("PostsUser")]
        [HttpGet]
        public async Task<IActionResult> GetPostsUser(string userID)
        {
             var postsOfUser = await _postManager.Users
                .Where(u => u.UserId == userID)
                .Select(u => new
                {
                    u.Id,
                    u.UserId,
                    u.Avatar,
                    u.Firstname,
                    u.LastName,
                    u.UserName,
                    u.Text,
                    u.UpVotes,
                    u.DownVotes,
                    u.CreatedAt,
                    u.DislikedByUserId,
                    u.LikedByUserId,
                    DislikedByUser = new { },
                    LikedByUser = new { }
                }).ToListAsync();
            if(postsOfUser.Count == 0) { return NotFound(); }
            return Ok(postsOfUser); 
        }

        /// <summary>
        /// Gets the posts of the following user.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>The posts of the following users, or a 404 Not Found error if no such post exists.</returns>
        /// <response code="200">Returns the posts from the following users.</response>
        /// <response code="404">If the user is not following anybody.</response>
        /// <response code="500">If an exception occurs while retrieving the user.</response>
        [Route("FollowedUsersPosts")]
        [HttpGet]
        public async Task<IActionResult> GetFollowedUsersPosts(int userID)
        {
            var postsOfFollowingUser =  await _context.Posts
                .Where(u => _context.Following
                    .Where(f => f.Id == userID)
                    .Select(f => f.FollowingUserId)
                    .Contains(u.UserId))
                .Select(u => new {
                    u.Id,
                    u.UserId,
                    u.Avatar,
                    u.Firstname,
                    u.LastName,
                    u.UserName,
                    u.Text,
                    u.UpVotes,
                    u.DownVotes,
                    u.CreatedAt,
                    u.DislikedByUserId,
                    u.LikedByUserId,
                    DislikedByUser = new { },
                    LikedByUser = new { }
                })
                .ToListAsync();
            if(postsOfFollowingUser.Count == 0) { return NotFound(); }
            return Ok(postsOfFollowingUser);
        }

        /// <summary>
        /// Gets a post with the specified ID.
        /// </summary>
        /// <param name="postID">The ID of the post to get.</param>
        /// <returns>The post of the specified ID, or a 404 Not Found error if no such post exists.</returns>
        /// <response code="200">Returns the post with the specified ID.</response>
        /// <response code="404">If the post with the specified ID is not found.</response>
        /// <response code="500">If an exception occurs while retrieving the user.</response>
        [Route("singlePost")]
        [HttpGet]
        public async Task<IActionResult> GetPost(string postID)
        {
            var post = await _postManager.Users
                .Where(u => u.Id == postID)
                .Select(u => new
                {
                    u.Id,
                    u.UserId,
                    u.Avatar,
                    u.Firstname,
                    u.LastName,
                    u.UserName,
                    u.Text,
                    u.UpVotes,
                    u.DownVotes,
                    u.CreatedAt,
                    u.DislikedByUserId,
                    u.LikedByUserId,
                    DislikedByUser = new { },
                    LikedByUser = new { }
                }).ToListAsync();
            if(post.Count == 0) { return NotFound(); }
            return Ok(post);
        }

    }
}
