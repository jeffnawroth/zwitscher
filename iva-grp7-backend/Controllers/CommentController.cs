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
public class CommentController: ControllerBase
{
    
    
        private readonly ApiDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(ApiDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        /// <summary>
        ///     Creates a new comment.
        /// </summary>
        /// <param name="comment">The comment to create.</param>
        /// <returns>The created comment.</returns>
        /// <response code="201">Returns the created comment.</response>
        /// <response code="400">If the comment data is invalid.</response>
        /// <response code="500">If an exception occurs while creating the comment.</response> 
        [HttpPost]
[ProducesResponseType(201, Type = typeof(CommentResult))]
[ProducesResponseType(400)]
[ProducesResponseType(500)]
        public async Task<ActionResult<PostResult>> CreateComment(CommentAdd commentAdd)
        {
            // Find the user
            var user = await _userManager.FindByIdAsync(commentAdd.UserId);

            if (user == null) return NotFound("User wurde nicht gefunden");

            var parentPost = await _context.Posts.Include(p => p.Comments).SingleOrDefaultAsync(p => p.Id == commentAdd.ParentPostId);

            if (parentPost == null) return NotFound("Übergeordneter Post wurde nicht gefunden");

            // Create a new comment
            var comment = new Comment
            {
                UserId = user.Id,
                User = user,
                Text = commentAdd.Text,
                Name = user.Name, // Add the user's name
                Username = user.UserName, // Add the user's username
                ParentPostId = commentAdd.ParentPostId,
            };

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

                    var postFile = new PostFile
                    {
                        Data = fileBytes,
                        PostId = comment.Id,
                        MediaType = fileSplit[0] // Medientyp speichern
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
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentResult>> GetComment(string id)
        {
            // Find the comment, include the Votes, PostFiles and nested Comments in the query
            var comment = await _context.Comments
                .Include(p => p.Votes)
                .Include(p => p.Files)
                .Include(p => p.Comments)  // Include nested comments
                .SingleOrDefaultAsync(p => p.Id == id);

            if (comment == null) return NotFound();

            // Create the CommentResult recursively
            var commentResult = await CreateCommentResult(comment);

            return commentResult;
        }
        private async Task<CommentResult> CreateCommentResult(Comment comment)
        {
            // User Informationen laden
            var user = await _userManager.FindByIdAsync(comment.UserId);

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
                UpVotes = comment.Votes.Where(v => v.IsUpvote).Select(v => v.UserId).ToList(),
                DownVotes = comment.Votes.Where(v => !v.IsUpvote).Select(v => v.UserId).ToList(),
                Files = comment.Files?.Select(f => $"{f.MediaType},{Convert.ToBase64String(f.Data)}").ToList() ??
                        new List<string>(),
                Edited = comment.Edited,
                Comments = new List<CommentResult>(),
                ParentPostId = comment.ParentPostId
            };

            foreach(var nestedComment in comment.Comments)
            {
                var nestedCommentResult = await CreateCommentResult(nestedComment);
                commentResult.Comments.Add(nestedCommentResult);
            }

            return commentResult;
        }
        
        /// <summary>
        ///     Deletes a comment and all nested comments.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the comment is successfully deleted.</response>
        /// <response code="404">If the comment is not found.</response>
        /// <response code="500">If an exception occurs while deleting the comment.</response>
        /// <remarks>
        /// Beispiel für eine Anforderung:
        /// DELETE /api/comments/{id}
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            var comment = await _context.Comments.Include(c => c.Comments)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            await DeleteNestedComments(comment);

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent(); // Response 204
        }

        private async Task DeleteNestedComments(Comment comment)
        {
            foreach (var nestedComment in comment.Comments)
            {
                await DeleteNestedComments(nestedComment);
                _context.Comments.Remove(nestedComment);
            }
        }
        

}