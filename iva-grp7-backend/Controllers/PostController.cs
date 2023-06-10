using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

namespace iva_grp7_backend.Controllers;
    

    /// <summary>
    /// A controller for managing posts.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class PostController: ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(ApiDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<ActionResult<Post>> CreatePost(PostAdd postAdd)
        {
            var user = await _userManager.FindByIdAsync(postAdd.UserId);

            Console.WriteLine(user.Name);
            
            if (user.Name == null)
            {
                return NotFound("Name ist null");
            }
            
            
            var post = new Post
            {
                UserId = postAdd.UserId,
                Text = postAdd.text,
                Files = postAdd.Files,
                UpVotes = 0,
                DownVotes = 0,
                Name = user.Name,
                Username = user.UserName,
                Avatar = user.Avatar,
                
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }
        
        /// <summary>
        /// Gets all posts.
        /// </summary>
        /// <returns>A list of all posts.</returns>
        /// <response code="200">Returns the list of posts.</response>
        /// <response code="500">If an exception occurs while retrieving the posts.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Post>))]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllPublicPosts()
        {
            // Alle Posts laden
            var posts = await _context.Posts.ToListAsync();

            // Für jeden Post die User-Informationen laden und setzen
            foreach (var post in posts)
            {
                var user = await _userManager.FindByIdAsync(post.UserId);
                if (user != null)
                {
                    post.Name = user.Name; // Angenommen, dass es in Ihrer IdentityUser Erweiterung ein "Name" Feld gibt
                    post.Username = user.UserName;
                    post.Avatar = user.Avatar; // Sie müssen die Logik zum Abrufen des Avatar-Bildpfades implementieren
                }
            }

            return posts;
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
        public async Task<ActionResult<Post>> GetPost(string id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            // User Informationen laden
            var user = await _userManager.FindByIdAsync(post.UserId);
            if (user != null)
            {
                post.Name = user.Name; // Angenommen, dass es in Ihrer IdentityUser Erweiterung ein "Name" Feld gibt
                post.Username = user.UserName;
                post.Avatar = user.Avatar; // Sie müssen die Logik zum Abrufen des Avatar-Bildpfades implementieren
            }

            return post;
        }
        
        /// <summary>
        /// Gets all posts from a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of all posts from the user.</returns>
        /// <response code="200">Returns the list of posts.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If an exception occurs while retrieving the posts.</response>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUser(string userId)
        {
            // Überprüfen, ob der Benutzer existiert
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Alle Posts des Benutzers laden
            var posts = await _context.Posts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            // Benutzerinformationen für jeden Post setzen
            foreach (var post in posts)
            {
                post.Name = user.Name; // Angenommen, dass es in Ihrer IdentityUser Erweiterung ein "Name" Feld gibt
                post.Username = user.UserName;
                post.Avatar = user.Avatar; // Sie müssen die Logik zum Abrufen des Avatar-Bildpfades implementieren
            }

            return posts;
        }
        
        /// <summary>
        /// Deletes a post.
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
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok("Post wurde erfolgreich gelöscht");
        }
        
        /// <summary>
        /// Updates a post.
        /// </summary>
        /// <param name="updatedPost">The updated post data.</param>
        /// <returns>The updated post.</returns>
        /// <response code="200">If the post is successfully updated.</response>
        /// <response code="404">If the post is not found.</response>
        /// <response code="500">If an exception occurs while updating the post.</response>
        [HttpPut]
        public async Task<IActionResult> UpdatePost(Post updatedPost)
        {
            var post = await _context.Posts.FindAsync(updatedPost.Id);
            if (post == null)
            {
                return NotFound("Post wurde nicht gefunden");
            }

            post.Text = updatedPost.Text;
            post.Files = updatedPost.Files;
            // Hier können Sie andere Felder hinzufügen, die aktualisiert werden sollen...
            // post.SomeField = updatedPost.SomeField;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Posts.Any(e => e.Id == updatedPost.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        
    }