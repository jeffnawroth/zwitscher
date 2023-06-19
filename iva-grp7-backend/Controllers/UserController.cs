using System.Security.Claims;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApiDbContext _context;

        /// <summary>
        /// Creates a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager to use for managing users.</param>
        public UserController(UserManager<ApplicationUser> userManager, ApiDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var currentUser = await _userManager.FindByEmailAsync(userId);
    
    if(currentUser == null)
    {
        return NotFound("Current user not found.");
    }

    if(currentUser.Role == null)
    {
        return NotFound("Current user role not found.");
    }
    
    var users = await _userManager.Users.Include(u => u.Followers).ThenInclude(f => f.Follower)
                                        .ThenInclude(u => u.Following)
                                        .Include(u => u.Interests)
                                        .ToListAsync();
    
    var filteredUsers = new List<User>();
    
    if (currentUser.Role == Role.Admin)
    {
        foreach (var user in users)
        {
            // Check if the lists are null before executing the Select-Method
            var followerIds = user.Followers != null 
                ? user.Followers.Select(f => f.FollowerId).ToList() 
                : new List<string>();

            var followingIds = user.Following != null 
                ? user.Following.Select(f => f.FollowingId).ToList() 
                : new List<string>();
        
            var interests = user.Interests != null 
                ? user.Interests.Select(f => f.Interest).ToList() 
                : new List<string>();
            
            var filteredUser = new User()
            {
                Id = user.Id,
                Avatar = user.Avatar,
                Role = user.Role,
                Username = user.UserName,
                Name = user.Name,
                Email = user.Email,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                CreatedAt = user.CreatedAt,
                Bio = user.Bio,
                Locked = user.Locked,
                Followers = followerIds,
                Following = followingIds,
                Interests = interests
            };

            filteredUsers.Add(filteredUser);
        }
    }

    if (currentUser.Role == Role.Moderator)
    {
        foreach (var user in users)
        {
            // Check if the lists are null before executing the Select-Method
            var followerIds = user.Followers != null 
                ? user.Followers.Select(f => f.FollowerId).ToList() 
                : new List<string>();

            var followingIds = user.Following != null 
                ? user.Following.Select(f => f.FollowingId).ToList() 
                : new List<string>();
        
            var interests = user.Interests != null 
                ? user.Interests.Select(f => f.Interest).ToList() 
                : new List<string>();
            
            if (user.Role == Role.User)
            {
                var filteredUser = new User
                {
                    Id = user.Id,
                    Avatar = user.Avatar,
                    Role = user.Role,
                    Username = user.UserName,
                    Name = user.Name,
                    Email = user.Email,
                    Gender = user.Gender,
                    BirthDate = user.BirthDate,
                    CreatedAt = user.CreatedAt,
                    Bio = user.Bio,
                    Locked = user.Locked,
                    Followers = followerIds,
                    Following = followingIds,
                    Interests = interests
                };

                filteredUsers.Add(filteredUser);
            }
        }
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
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        // Retrieve a user based on their ID
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Followers).ThenInclude(f => f.Follower)
                .Include(u => u.Following).ThenInclude(f => f.Following)
                .Include(u => u.Interests)
                .FirstOrDefaultAsync(u => u.Id == id);

            // If no user is found with the given ID, return a 404 Not Found response
            if (user == null)
            {

                return NotFound();
            }
            
                // Überprüfe hier die gewünschten Attribute des Benutzers und füge ihn zur Liste "filteredUsers" hinzu, wenn die Bedingungen erfüllt sind.
                // Beispiel: Wenn der Benutzer das Attribut "isActive" haben muss und das Attribut "isAdmin" nicht vorhanden sein darf:
                // Check if the lists are null before executing the Select-Method
                var followerIds = user.Followers != null 
                    ? user.Followers.Select(f => f.FollowerId).ToList() 
                    : new List<string>();

                var followingIds = user.Following != null 
                    ? user.Following.Select(f => f.FollowingId).ToList() 
                    : new List<string>();
        
                var interests = user.Interests != null 
                    ? user.Interests.Select(f => f.Interest).ToList() 
                    : new List<string>();
                
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
                    CreatedAt = user.CreatedAt,
                    Bio = user.Bio,
                    Locked = user.Locked,
                    Followers = followerIds,
                    Following = followingIds,
                    Interests = interests
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
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        // Retrieve a user based on their username
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userManager.Users
                .Include(u => u.Followers).ThenInclude(f => f.Follower)
                .Include(u => u.Following).ThenInclude(f => f.Following)
                .Include(u => u.Interests)
                .FirstOrDefaultAsync(u => u.UserName == username);

            // If no user is found with the given username, return a 404 Not Found response
            if (user == null)
            {

                return NotFound();
            }
            
            // Überprüfe hier die gewünschten Attribute des Benutzers und füge ihn zur Liste "filteredUsers" hinzu, wenn die Bedingungen erfüllt sind.
            // Beispiel: Wenn der Benutzer das Attribut "isActive" haben muss und das Attribut "isAdmin" nicht vorhanden sein darf:
            // Check if the lists are null before executing the Select-Method
            var followerIds = user.Followers != null 
                ? user.Followers.Select(f => f.FollowerId).ToList() 
                : new List<string>();

            var followingIds = user.Following != null 
                ? user.Following.Select(f => f.FollowingId).ToList() 
                : new List<string>();
        
            var interests = user.Interests != null 
                ? user.Interests.Select(f => f.Interest).ToList() 
                : new List<string>();
            
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
                CreatedAt = user.CreatedAt,
                Bio = user.Bio,
                Locked = user.Locked,
                Followers = followerIds,
                Following = followingIds,
                Interests = interests
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
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        // Creates a new ApplicationUser
        public async Task<IActionResult> Create([FromBody] UserAdd user)
{
    // Checks if the model state is valid
    if (!ModelState.IsValid)
    {
        // If the model state is invalid, returns a bad request response with the validation errors
        return BadRequest(ModelState);
    }

    string password = user.Password;

    ApplicationUser applicationUser = new ApplicationUser()
    {
        Avatar = user.Avatar,
        Role = user.Role,
        UserName = user.Username,
        Name = user.Name,
        Email = user.Email,
        Gender = user.Gender,
        BirthDate = user.BirthDate,
        Bio = user.Bio,
    };

    var is_created = await _userManager.CreateAsync(applicationUser, password);

    if (!is_created.Succeeded)
    {
        // Get the errors from the result
        var errors = is_created.Errors.Select(e => e.Description);
        // Return a BadRequest response with the errors
        return BadRequest(errors);
    }

    if (is_created.Succeeded)
    {
        // If user has interests
        if (user.Interests != null)
        {
            // Initialize a list to store the UserInterests
            List<UserInterest> userInterests = new List<UserInterest>();

            foreach (string interestName in user.Interests)
            {
                // Create a new UserInterest that links the user and the interest
                UserInterest userInterest = new UserInterest
                {
                    Interest = interestName,
                    User = applicationUser
                };

                // Add the UserInterest to the list
                userInterests.Add(userInterest);
            }

            // Assign the list of UserInterests to the user's Interests
            applicationUser.Interests = userInterests;

            // Update the user in the database
            _context.Entry(applicationUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        var filteredUser = new User
        {
            Id = applicationUser.Id,
            Avatar = applicationUser.Avatar,
            Role = applicationUser.Role,
            Username = applicationUser.UserName,
            Name = applicationUser.Name,
            Email = applicationUser.Email,
            Gender = applicationUser.Gender,
            BirthDate = applicationUser.BirthDate,
            CreatedAt = applicationUser.CreatedAt,
            Bio = applicationUser.Bio,
            Locked = applicationUser.Locked,
            Followers = new List<string>(),
            Following = new List<string>(),
            Interests = user.Interests // Use the interests from the input, since they're the same as in the database
        };

        // If the user creation was successful, returns a created response with the newly created user object
        return CreatedAtAction(nameof(GetById), new {id = filteredUser.Id}, filteredUser);
    }

    // If the user creation was not successful, returns a bad request response with the error messages
    return BadRequest(is_created.Errors);
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
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        // Updates the ApplicationUser.
        public async Task<IActionResult> Update(string id, [FromBody] UserEdit user)
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
            existingUser.UserName = user.Username;
            existingUser.Email = user.Email;
            existingUser.Bio = user.Bio;
            existingUser.Gender = user.Gender;
            existingUser.Locked = user.Locked;
            existingUser.Name = user.Name;
            existingUser.BirthDate = user.BirthDate;
            existingUser.Role = user.Role;
        
            // If user has interests
            if (user.Interests != null)
            {
                // Delete existing interests of the user
                var existingInterests = _context.UserInterests.Where(ui => ui.UserId == existingUser.Id);
                _context.UserInterests.RemoveRange(existingInterests);
        
                // Initialize a list to store the UserInterests
                List<UserInterest> userInterests = new List<UserInterest>();
        
                foreach (string interestName in user.Interests)
                {
                    // Create a new UserInterest that links the user and the interest
                    UserInterest userInterest = new UserInterest
                    {
                        Interest = interestName,
                        User = existingUser
                    };
        
                    // Add the UserInterest to the list
                    userInterests.Add(userInterest);
                }
        
                // Assign the list of UserInterests to the user's Interests
                existingUser.Interests = userInterests;
            }
        
            // Handle password update
            if (user.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var change = await _userManager.ResetPasswordAsync(existingUser, token, user.Password);
                if (!change.Succeeded)
                {
                    return BadRequest("Password konnte nicht geändert werden");
                }
            }
        
            // Update the existing user.
            var result = await _userManager.UpdateAsync(existingUser);
        
            // If the update is successful, return an OK response with the updated user object.
            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();
                return Ok("User wurde erfolgreich aktualisiert");
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
