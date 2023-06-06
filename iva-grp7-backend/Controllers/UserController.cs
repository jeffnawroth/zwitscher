using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iva_grp7_backend.Controllers;

    /// <summary>
    /// A controller for managing users.
    /// </summary>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Creates a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager to use for managing users.</param>
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        /// <summary>
        /// Gets a list of all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="500">If an exception occurs while retrieving the users.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<User>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            var filteredUsers = new List<User>();

            foreach (var user in users)
            {
                // Überprüfe hier die gewünschten Attribute des Benutzers und füge ihn zur Liste "filteredUsers" hinzu, wenn die Bedingungen erfüllt sind.
                // Beispiel: Wenn der Benutzer das Attribut "isActive" haben muss und das Attribut "isAdmin" nicht vorhanden sein darf:
                var filteredUser = new User
                    {
                        // Wähle hier die gewünschten Attribute des Benutzers aus und weise sie dem entsprechenden Attribut im filteredUser-Objekt zu.
                        // Beispiel: Nur die Attribute "Name" und "Email" sollen in die filteredUsers-Liste aufgenommen werden:
                        Id = user.Id,
                        Avatar = user.Avatar,
                        Role = user.Role,
                        Username = user.UserName,
                        Name = user.Name,
                        Email = user.Email,
                        Gender = user.Gender,
                        BirthDate = user.BirthDate,
                        Followers = user.Followers,
                        Following = user.Following,
                        LikedPosts = user.LikedPosts,
                        DislikedPosts = user.DislikedPosts,
                        CreatedAt = user.CreatedAt,
                        Bio = user.Bio,
                        Interests = user.Interests,
                        Locked = user.Locked
                    };

                    filteredUsers.Add(filteredUser);
                }
            return Ok(filteredUsers);
        }

        /// <summary>
        /// Gets a user with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the user to get.</param>
        /// <returns>The user with the specified ID, or a 404 Not Found error if no such user exists.</returns>
        /// <response code="200">Returns the user with the specified ID.</response>
        /// <response code="404">If the user with the specified ID is not found.</response>
        /// <response code="500">If an exception occurs while retrieving the user.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ApplicationUser))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        // Retrieve a user based on their ID
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            // If no user is found with the given ID, return a 404 Not Found response
            if (user == null)
            {

                return NotFound();
            }
            
                // Überprüfe hier die gewünschten Attribute des Benutzers und füge ihn zur Liste "filteredUsers" hinzu, wenn die Bedingungen erfüllt sind.
                // Beispiel: Wenn der Benutzer das Attribut "isActive" haben muss und das Attribut "isAdmin" nicht vorhanden sein darf:
                var filteredUser = new User
                {
                    // Wähle hier die gewünschten Attribute des Benutzers aus und weise sie dem entsprechenden Attribut im filteredUser-Objekt zu.
                    // Beispiel: Nur die Attribute "Name" und "Email" sollen in die filteredUsers-Liste aufgenommen werden:
                    Id = user.Id,
                    Avatar = user.Avatar,
                    Role = user.Role,
                    Username = user.UserName,
                    Name = user.Name,
                    Email = user.Email,
                    Gender = user.Gender,
                    BirthDate = user.BirthDate,
                    Followers = user.Followers,
                    Following = user.Following,
                    LikedPosts = user.LikedPosts,
                    DislikedPosts = user.DislikedPosts,
                    CreatedAt = user.CreatedAt,
                    Bio = user.Bio,
                    Interests = user.Interests,
                    Locked = user.Locked
                };
                
            // If a user is found with the given ID, return a 200 OK response with the user object as the response body
            return Ok(filteredUser);
        }
        
        /// <summary>
        /// Gets a user with the username.
        /// </summary>
        /// <param name="username">The username of the user to get.</param>
        /// <returns>The user with the specified username, or a 404 Not Found error if no such user exists.</returns>
        /// <response code="200">Returns the user with the specified ID.</response>
        /// <response code="404">If the user with the username is not found.</response>
        /// <response code="500">If an exception occurs while retrieving the user.</response>
        [HttpGet("GetByUsername/{username}")]
        [ProducesResponseType(200, Type = typeof(ApplicationUser))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        // Retrieve a user based on their username
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            // If no user is found with the given username, return a 404 Not Found response
            if (user == null)
            {

                return NotFound();
            }
            
            // Überprüfe hier die gewünschten Attribute des Benutzers und füge ihn zur Liste "filteredUsers" hinzu, wenn die Bedingungen erfüllt sind.
            // Beispiel: Wenn der Benutzer das Attribut "isActive" haben muss und das Attribut "isAdmin" nicht vorhanden sein darf:
            var filteredUser = new User
            {
                // Wähle hier die gewünschten Attribute des Benutzers aus und weise sie dem entsprechenden Attribut im filteredUser-Objekt zu.
                // Beispiel: Nur die Attribute "Name" und "Email" sollen in die filteredUsers-Liste aufgenommen werden:
                Id = user.Id,
                Avatar = user.Avatar,
                Role = user.Role,
                Username = user.UserName,
                Name = user.Name,
                Email = user.Email,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                Followers = user.Followers,
                Following = user.Following,
                LikedPosts = user.LikedPosts,
                DislikedPosts = user.DislikedPosts,
                CreatedAt = user.CreatedAt,
                Bio = user.Bio,
                Interests = user.Interests,
                Locked = user.Locked
            };

            // If a user is found with the given username, return a 200 OK response with the user object as the response body
            return Ok(filteredUser);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>The created user, or a 400 Bad Request error if the user is invalid.</returns>
        /// <response code="201">Returns the newly created user.</response>
        /// <response code="400">If the user is invalid.</response>
        /// <response code="500">If an exception occurs while creating the user.</response>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ApplicationUser))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        // Creates a new ApplicationUser
        public async Task<IActionResult> Create([FromBody] ApplicationUser user)
        {
            // Checks if the model state is valid
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, returns a bad request response with the validation errors
                return BadRequest(ModelState);
            }
            
            // Hashes the user's password using the PasswordHasher service injected into the class and sets the hash as the user's password hash
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, user.PasswordHash);

            // Checks if a user with the same email already exists in the system
            var user_exist = await _userManager.FindByEmailAsync(user.Email);
            if (user_exist != null)
            {
                // If a user with the same email already exists, returns a bad request response with an error message
                return BadRequest(new AuthResult()
                {
                    
                });
            }

            // Creates a new user in the system using the UserManager service injected into the class
            var result = await _userManager.CreateAsync(user);

            // Adds the user to the role specified in their role property
            await _userManager.AddToRoleAsync(user, $"{user.Role}");
            if (result.Succeeded)
            {
                // If the user creation was successful, returns a created response with the newly created user object
                return CreatedAtAction(nameof(GetById), new {id = user.Id}, user);
            }

            // If the user creation was not successful, returns a bad request response with the error messages
            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="user">The updated user information.</param>
        /// <returns>The updated user, or a 400 Bad Request error if the user is invalid or a 404 Not Found error if no such user exists.</returns>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="400">If the user is invalid.</response>
        /// <response code="404">If the user with the specified ID is not found.</response>
        /// <response code="500">If an exception occurs while updating the user.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ApplicationUser))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        // Updates the ApplicationUser.
        public async Task<IActionResult> Update(string id, [FromBody] ApplicationUser user)
        {
            // Check if the given id matches the user's id.
            if (id != user.Id)
            {
                // Return bad request if they don't match.
                return BadRequest();
            }

            // Check if the user object is valid.
            if (!ModelState.IsValid)
            {
                // Return bad request with model state errors if it's not valid.
                return BadRequest(ModelState);
            }

            // Find the existing user based on the id.
            var existingUser = await _userManager.FindByIdAsync(id);

            // Return not found if the user doesn't exist.
            if (existingUser == null)
            {
                return NotFound();
            }

            // Update the user properties with the properties from the given user object.
            existingUser.Id = user.Id;
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.ConcurrencyStamp = user.ConcurrencyStamp;
            existingUser.NormalizedUserName = user.NormalizedUserName;
            existingUser.NormalizedEmail = user.NormalizedEmail;
            existingUser.EmailConfirmed = user.EmailConfirmed;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.SecurityStamp = user.SecurityStamp;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            existingUser.TwoFactorEnabled = user.TwoFactorEnabled;
            existingUser.LockoutEnd = user.LockoutEnd;
            existingUser.LockoutEnabled = user.LockoutEnabled;
            existingUser.AccessFailedCount = user.AccessFailedCount;
            existingUser.Bio = user.Bio;
            existingUser.Followers = user.Followers;
            existingUser.Following = user.Following;
            existingUser.Gender = user.Gender;
            existingUser.Interests = user.Interests;
            existingUser.Locked = user.Locked;
            existingUser.Name = user.Name;
            existingUser.BirthDate = user.BirthDate;
            existingUser.LikedPosts = user.LikedPosts;
            existingUser.DislikedPosts = user.DislikedPosts;
            
            // Check if a user with the given email already exists.
            var userExist = await _userManager.FindByEmailAsync(existingUser.Email);

            // If user exists and the email is the same as the one provided, return a bad request with an error message.
            if (userExist != null && userExist.Email == user.Email)
            {
                return BadRequest(new AuthResult()
                {
                    
                });
            }

            // Update the existing user.
            var result = await _userManager.UpdateAsync(existingUser);

            // If the update is successful, return an OK response with the updated user object.
            if (result.Succeeded)
            {
                return Ok(user);
            }

            // If the update fails, return a bad request with the errors from the result object.
            return BadRequest(result.Errors);

        }

        /// <summary>
        /// Deletes a user with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A 200 OK response if the user was deleted successfully, or a 404 Not Found error if no such user exists.</returns>
        /// <response code="204">If the user is successfully deleted.</response>
        /// <response code="404">If the user with the specified ID is not found.</response>
        /// <response code="500">If an exception occurs while deleting the user.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        // This method deletes a user with a given id.
        public async Task<IActionResult> Delete(string id)
        {
            // Find the user with the given id using the UserManager.
            var user = await _userManager.FindByIdAsync(id);
            // If the user is not found, return a 404 Not Found response.
            if (user == null)
            {
                return NotFound();
            }

            // Attempt to delete the user using the UserManager.
            var result = await _userManager.DeleteAsync(user);

            // If the deletion is successful, return a 200 OK response.
            if (result.Succeeded)
            {
                return Ok("Der User wurde erfolgreich gelöscht");
            }

            // If the deletion is not successful, return a 400 Bad Request response and include any errors that occurred.
            return BadRequest(result.Errors);

        }
    }
