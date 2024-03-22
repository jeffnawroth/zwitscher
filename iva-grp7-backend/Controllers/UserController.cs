using System.Security.Claims;
using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iva_grp7_backend.Controllers;

/// <summary>
///     A controller for managing users.
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    ///     Creates a new instance of the <see cref="UserController" /> class.
    /// </summary>
    /// <param name="userManager">The user manager to use for managing users.</param>
    public UserController(UserManager<ApplicationUser> userManager, ApiDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }


    /// <summary>
    ///     Gets a list of all users.
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

    // Check if current user exists
    if (currentUser == null) return NotFound("Current user not found.");

    // Check if current user role is set
    if (currentUser.Role == null) return NotFound("Current user role not found.");

    // Get all users with their respective followers, following and interests
    var users = await _userManager.Users
        .Include(u => u.Followers).ThenInclude(f => f.Follower)
        .Include(u => u.Following).ThenInclude(f => f.Following)
        .Include(u => u.Interests)
        .Select(user => new
        {
            user.Id,
            user.Role,
            user.UserName,
            user.Name,
            user.Email,
            user.Gender,
            user.BirthDate,
            user.CreatedAt,
            user.Bio,
            user.Locked,
            Followers = user.Followers.Select(f => f.FollowerId),
            Following = user.Following.Select(f => f.FollowingId),
            Interests = user.Interests.Select(i => i.Interest)
        })
        .ToListAsync();

    var filteredUsers = new List<User>();

    // Filter users if current user is an Admin or Moderator
    if (currentUser.Role == Role.Admin || currentUser.Role == Role.Moderator)
    {
        foreach (var user in users)
        {
            var filteredUser = new User
            {
                Id = user.Id,
                Role = user.Role,
                Username = user.UserName,
                Name = user.Name,
                Email = user.Email,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                CreatedAt = user.CreatedAt,
                Bio = user.Bio,
                Locked = user.Locked,
                Followers = user.Followers.ToList(),
                Following = user.Following.ToList(),
                Interests = user.Interests.ToList()
            };

            filteredUsers.Add(filteredUser);
        }
    }

    return Ok(filteredUsers);
}



    /// <summary>
    ///     Gets a user with the specified ID.
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
    // Fetch the user with the given ID including their followers, following and interests
    var user = await _userManager.Users
        .Include(u => u.Followers).ThenInclude(f => f.Follower)
        .Include(u => u.Following).ThenInclude(f => f.Following)
        .Include(u => u.Interests)
        .FirstOrDefaultAsync(u => u.Id == id);

    // If no user is found with the given ID, return a 404 Not Found response
    if (user == null) return NotFound();

    // Check desired attributes of the user and add them to the "filteredUsers" list if the conditions are met.
    // Example: The user must have the attribute "isActive" and the attribute "isAdmin" must not exist:
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
        // Select desired user attributes and assign them to the corresponding attribute in the filteredUser object.
        // Example: Only the attributes "Name" and "Email" should be included in the filteredUsers list:
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
    ///     Gets a user with the username.
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
    // Fetch the user with the given username including their followers, following and interests
    var user = await _userManager.Users
        .Include(u => u.Followers).ThenInclude(f => f.Follower)
        .Include(u => u.Following).ThenInclude(f => f.Following)
        .Include(u => u.Interests)
        .FirstOrDefaultAsync(u => u.UserName == username);

    // If no user is found with the given username, return a 404 Not Found response
    if (user == null) return NotFound();

    // Check desired attributes of the user and add them to the "filteredUsers" list if the conditions are met.
    // Example: The user must have the attribute "isActive" and the attribute "isAdmin" must not exist:
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
        // Select desired user attributes and assign them to the corresponding attribute in the filteredUser object.
        // Example: Only the attributes "Name" and "Email" should be included in the filteredUsers list:
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
    ///     Creates a new user.
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
    // Check if the model state is valid
    if (!ModelState.IsValid)
        // If the model state is invalid, return a BadRequest response with the validation errors
        return BadRequest(ModelState);

    byte[] avatarData = null;
    if (!string.IsNullOrEmpty(user.Avatar))
    {
        var avatarSplit = user.Avatar.Split(',');
        if (avatarSplit.Length == 2 && avatarSplit[0].StartsWith("data:") && avatarSplit[0].EndsWith("base64"))
            try
            {
                avatarData = Convert.FromBase64String(avatarSplit[1]);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Avatar format. Please provide a Base64 string.");
            }
        else
            return BadRequest("Invalid Avatar format. Please provide a data URI with base64 data.");
    }

    // Rest of the user information
    var password = user.Password;

    var applicationUser = new ApplicationUser
    {
        Avatar = avatarData,
        Role = user.Role,
        UserName = user.Username,
        Name = user.Name,
        Email = user.Email,
        Gender = user.Gender,
        BirthDate = user.BirthDate,
        Bio = user.Bio
    };

    var is_created = await _userManager.CreateAsync(applicationUser, password);

    if (!is_created.Succeeded)
    {
        // Fetch the errors from the result
        var errors = is_created.Errors.Select(e => e.Description);
        // Return a BadRequest response with the errors
        return BadRequest(errors);
    }

    if (is_created.Succeeded)
    {
        // If the user has interests
        if (user.Interests != null)
        {
            // Initialize a list to store the UserInterests
            var userInterests = new List<UserInterest>();

            foreach (var interestName in user.Interests)
            {
                // Create a new UserInterest that links the user and the interest
                var userInterest = new UserInterest
                {
                    Interest = interestName,
                    User = applicationUser
                };

                // Add the UserInterest to the list
                userInterests.Add(userInterest);
            }

            // Assign the list of UserInterests to the user's interests
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
            Interests = user
                .Interests // Use the interests from the input as they are the same as in the database
        };

        // If the user creation was successful, return a created response with the newly created user object
        return CreatedAtAction(nameof(GetById), new {id = filteredUser.Id}, filteredUser);
    }

    // If the user creation was not successful, return a BadRequest response with the error messages
    return BadRequest(is_created.Errors);
}



    /// <summary>
    ///     Updates a users email.
    /// </summary>
    /// <param name="new_email">The new email of the user.</param>
    /// <returns>An Ok if everything went correctly, 404 if the user is not found.</returns>
    /// <response code="200">Returns the message that the Email got updated correctly.</response>
    /// <response code="404">If the user is not found.</response>
    /// <response code="500">If an exception occurs while creating the user.</response>
    [HttpPut("EmailChange")]
    public async Task<IActionResult> UpdateEmail(string new_email)
    {
        // Get the email of the current user from the user claims
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
    
        // Use the UserManager service to fetch the user object from the database
        var currentUser = await _userManager.FindByEmailAsync(userEmail);

        // If no user is found, return NotFound
        if (currentUser == null) return NotFound("User not found");
    
        // Try to update the user's email
        try
        {
            await _userManager.SetEmailAsync(currentUser, new_email);
        }
        // Catch a DbUpdateConcurrencyException if there are database concurrency issues
        catch (DbUpdateConcurrencyException)
        {
            // Check if the user still exists
            if (!_userManager.Users.Any(u => u.Id == currentUser.Id))
                // If not, return NotFound
                return NotFound();
        
            // Otherwise, re-throw the exception
            throw;
        }

        // If the email update is successful, return an OK response with a success message
        return Ok("Email has been updated");
    }


    /// <summary>
    ///     Updates a users password.
    /// </summary>
    /// <param name="new_password">The new password of the user.</param>
    /// <returns>An Ok if everything went correctly, 404 if the user is not found.</returns>
    /// <response code="200">Returns the message that the passowrd got updated correctly.</response>
    /// <response code="400">There was an error updating the password.</response>
    /// <response code="404">If the user is not found.</response>
    /// <response code="500">If an exception occurs while creating the user.</response>
    [HttpPut("PasswordChange")]
    public async Task<IActionResult> UpdatePassword(string new_password)
    {
        // Get the email of the currently authenticated user from the user claims
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Use the UserManager service to fetch the user object from the database
        var currentUser = await _userManager.FindByEmailAsync(userEmail);

        // If no user is found, return NotFound
        if (currentUser == null) return NotFound("User not found");

        try
        {
            // Generate a password reset token for the current user
            var token = await _userManager.GeneratePasswordResetTokenAsync(currentUser);
        
            // Use the reset token to change the user's password to the new password
            var change = await _userManager.ResetPasswordAsync(currentUser, token, new_password);

            // If the password reset is unsuccessful, return BadRequest
            if (!change.Succeeded) return BadRequest("Password konnte nicht geändert werden");
        }
        // Catch a DbUpdateConcurrencyException if there are database concurrency issues
        catch (DbUpdateConcurrencyException)
        {
            // Check if the user still exists
            if (!_userManager.Users.Any(u => u.Id == currentUser.Id))
                // If not, return NotFound
                return NotFound();
        
            // Otherwise, re-throw the exception
            throw;
        }

        // If the password update is successful, return an OK response with a success message
        return Ok("Password wurde aktualisiert");
    }



    /// <summary>
    ///     Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="user">The updated user information.</param>
    /// <returns>
    ///     The updated user, or a 400 Bad Request error if the user is invalid or a 404 Not Found error if no such user
    ///     exists.
    /// </returns>
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
    // Check if provided user id matches the id from the request body
    if (id != user.Id) return BadRequest();

    // Check the validation of model state
    if (!ModelState.IsValid) return BadRequest(ModelState);

    // Find the user to be updated
    var existingUser = await _userManager.FindByIdAsync(id);

    // If the user is not found, return NotFound
    if (existingUser == null) return NotFound();

    // If there is a new avatar in the request
    if (user.Avatar != null)
    {
        try
        {
            byte[] avatarBytes;

            // Check if the Avatar string contains a media type prefix
            if (user.Avatar.Contains(','))
            {
                var fileSplit = user.Avatar.Split(',');
                try
                {
                    avatarBytes = Convert.FromBase64String(fileSplit[1]);
                }
                catch (FormatException)
                {
                    // If the conversion from Base64 string to byte array fails, return BadRequest
                    return BadRequest("Invalid Avatar format. Please provide a valid Base64 string.");
                }
            }
            else
            {
                try
                {
                    avatarBytes = Convert.FromBase64String(user.Avatar);
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid Avatar format. Please provide a valid Base64 string.");
                }
            }

            // Check if the user avatar is not already the same as the new avatar
            if (existingUser.Avatar == null || !existingUser.Avatar.SequenceEqual(avatarBytes))
                existingUser.Avatar = avatarBytes;
        }
        catch (Exception e)
        {
            // If any exception occurs, return BadRequest
            return BadRequest("Fehler beim Aktualisieren des Avatars: " + e.Message);
        }
    }

    // Update the user details
    existingUser.UserName = user.Username;
    existingUser.Email = user.Email;
    existingUser.Bio = user.Bio;
    existingUser.Gender = user.Gender;
    existingUser.Locked = user.Locked;
    existingUser.Name = user.Name;
    existingUser.BirthDate = user.BirthDate;
    existingUser.Role = user.Role;

    // If the user has new interests
    if (user.Interests != null)
    {
        // Find and remove the old interests from the database
        var existingInterests = _context.UserInterests.Where(ui => ui.UserId == existingUser.Id);
        _context.UserInterests.RemoveRange(existingInterests);

        var userInterests = new List<UserInterest>();

        // Add the new interests to the database
        foreach (var interestName in user.Interests)
        {
            var userInterest = new UserInterest
            {
                Interest = interestName,
                User = existingUser
            };

            userInterests.Add(userInterest);
        }

        existingUser.Interests = userInterests;
    }

    // Attempt to update the user
    var result = await _userManager.UpdateAsync(existingUser);

    if (result.Succeeded)
    {
        // If the update is successful, save the changes and return Ok
        await _context.SaveChangesAsync();
        return Ok("User wurde erfolgreich aktualisiert");
    }

    // If the update is unsuccessful, return the errors
    return BadRequest(result.Errors);
}



    /// <summary>
    ///     Deletes a user with the specified ID.
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
    // Find the user to be deleted
    var user = await _userManager.FindByIdAsync(id);

    // If the user is not found, return NotFound
    if (user == null) return NotFound();

    // Get the followers of the user
    var userFollowers = await _context.UserFollowers.Where(uf => uf.UserId == id).ToListAsync();
    // Remove all followers of the user
    _context.UserFollowers.RemoveRange(userFollowers);

    // Get the users that this user is following
    var followedByUsers = await _context.UserFollowers.Where(uf => uf.FollowerId == id).ToListAsync();
    // Remove all users that this user is following
    _context.UserFollowers.RemoveRange(followedByUsers);

    // Get the users that are following this user
    var userFollowingUsers = await _context.UserFollowings.Where(uf => uf.UserId == id).ToListAsync();
    // Remove all users that are following this user
    _context.UserFollowings.RemoveRange(userFollowingUsers);

    // Get the users that this user is followed by
    var userFollowedUsers = await _context.UserFollowings.Where(uf => uf.FollowingId == id).ToListAsync();
    // Remove all users that this user is followed by
    _context.UserFollowings.RemoveRange(userFollowedUsers);

    // Get the interests of the user
    var userInterests = await _context.UserInterests.Where(ui => ui.UserId == id).ToListAsync();
    // Remove all interests of the user
    _context.UserInterests.RemoveRange(userInterests);

    // Get the post votes of the user
    var userPostVotes = await _context.PostVotes.Where(pv => pv.UserId == id).ToListAsync();
    // Remove all post votes of the user
    _context.PostVotes.RemoveRange(userPostVotes);

    // Save all the changes made above
    await _context.SaveChangesAsync();

    // Try to delete the user
    var result = await _userManager.DeleteAsync(user);

    // If the user was successfully deleted, return an Ok response
    if (result.Succeeded) return Ok("Der User wurde erfolgreich gelöscht");

    // If the user deletion was not successful, return the errors
    return BadRequest(result.Errors);
}


    /// <summary>
    ///     Allows a user to follow another user.
    /// </summary>
    /// <param name="id">The ID of the user to be followed.</param>
    /// <returns>A 200 OK response on successful follow, a 404 Not Found error if no such user exists to be followed.</returns>
    /// <response code="200">Returns when the current user successfully follows the specified user.</response>
    /// <response code="404">If the user with the specified ID to be followed is not found.</response>
    /// <response code="500">If an exception occurs while following the user.</response>
    [HttpPost("{id}/follow")]
    public async Task<IActionResult> Follow(string id)
    {
        // Get the email of the current authenticated user from the claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Find the current user using the email
        var currentUser = await _userManager.FindByEmailAsync(userId);

        // If the current user is not found, return NotFound
        if (currentUser == null) return NotFound("Current user not found.");

        // Find the user to be followed
        var userToFollow = await _userManager.FindByIdAsync(id);
        // If the user to be followed is not found, return NotFound
        if (userToFollow == null) return NotFound();

        // Create a new UserFollowing record for the current user following the other user
        var userFollowing = new UserFollowing
        {
            UserId = currentUser.Id,
            FollowingId = userToFollow.Id
        };
        // Add the UserFollowing record to the database
        _context.UserFollowings.Add(userFollowing);

        // Create a new UserFollower record for the user being followed by the current user
        var userFollower = new UserFollower
        {
            UserId = userToFollow.Id,
            FollowerId = currentUser.Id
        };
        // Add the UserFollower record to the database
        _context.UserFollowers.Add(userFollower);

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Return Ok status to indicate that the user has successfully followed the other user
        return Ok("Dem User wird nun erfolgreich gefolgt");
    }


    /// <summary>
    ///     Allows a user to unfollow another user.
    /// </summary>
    /// <param name="id">The ID of the user to be unfollowed.</param>
    /// <returns>A 200 OK response on successful unfollow, a 404 Not Found error if no such user exists to be unfollowed.</returns>
    /// <response code="200">Returns when the current user successfully unfollows the specified user.</response>
    /// <response code="404">If the user with the specified ID to be unfollowed is not found.</response>
    /// <response code="500">If an exception occurs while unfollowing the user.</response>
    [HttpPost("{id}/unfollow")]
    public async Task<IActionResult> Unfollow(string id)
    {
        // Get the email of the current authenticated user from the claims
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Find the current user using the email
        var currentUser = await _userManager.FindByEmailAsync(userId);

        // If the current user is not found, return NotFound
        if (currentUser == null) return NotFound("Current user not found.");

        // Find the user to be unfollowed
        var userToUnfollow = await _userManager.FindByIdAsync(id);
        // If the user to be unfollowed is not found, return NotFound
        if (userToUnfollow == null) return NotFound();

        // Find the UserFollowing record for the current user following the other user
        var userFollowing = await _context.UserFollowings.FirstOrDefaultAsync(
            uf => uf.UserId == currentUser.Id && uf.FollowingId == userToUnfollow.Id
        );

        // Find the UserFollower record for the user being followed by the current user
        var userFollower = await _context.UserFollowers.FirstOrDefaultAsync(
            uf => uf.UserId == userToUnfollow.Id && uf.FollowerId == currentUser.Id
        );

        // If the UserFollowing record exists, remove it from the database
        if (userFollowing != null) _context.UserFollowings.Remove(userFollowing);

        // If the UserFollower record exists, remove it from the database
        if (userFollower != null) _context.UserFollowers.Remove(userFollower);

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Return Ok status to indicate that the user has successfully unfollowed the other user
        return Ok("Dem User wurde nun erfolgreich entfolgt");
    }
    
    /// <summary>
    ///     Gets a list of all followed users with specified attributes.
    /// </summary>
    /// <returns>A list of all followed users from the current user.</returns>
    /// <response code="200">Returns the list of followed users with the specified attributes.</response>
    /// <response code="500">If an exception occurs while retrieving the users.</response>
    [HttpGet("FollowedUsersLight")]
    public async Task<ActionResult<IEnumerable<UserLight>>> GetFollowerdUsersLight()
    {
        // A lightweight user should only contain: id, avatar, name, username
        // Retrieve the email of the current authenticated user from the claims
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Find the current user using the email
        var currentUser = await _userManager.FindByEmailAsync(userEmail);
        // Create a list to store the lightweight versions of the followed users
        var followingUsers = new List<UserLight>();

        // If the current user is not found, return NotFound
        if (currentUser == null) return NotFound("Current user not found.");

        // Get the IDs of the users that the current user is following
        var followingIds = await _context.UserFollowings
            .Where(f => f.UserId == currentUser.Id)
            .Select(f => f.FollowingId)
            .ToListAsync();

        // Retrieve the user records for the users being followed
        var followedUsers = await _userManager.Users
            .Where(u => followingIds.Contains(u.Id))
            .ToListAsync();

        // Convert the full user records into the lightweight format and add them to the list
        foreach (var user in followedUsers)
        {
            var followingUser = new UserLight
            {
                Id = user.Id,
                Avatar = user.Avatar,
                Username = user.UserName,
                Name = user.Name
            };
            followingUsers.Add(followingUser);
        }

        // Return the list of lightweight user records
        return followingUsers;
    }


    /// <summary>
    ///     Searches for users based on a given query string.
    /// </summary>
    /// <remarks>
    ///     The search is performed on the UserName and Name properties. The search is case-insensitive.
    /// </remarks>
    /// <param name="query">The string to search for.</param>
    /// <returns>A list of users matching the search criteria.</returns>
    /// <response code="200">Returns the found list of users.</response>
    /// <response code="404">If no user is found.</response>
    [HttpGet("search/{query}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<UserSearch>>> SearchUsers(string? query)
    {
        // Check if the query is null, empty, or whitespace
        if (string.IsNullOrWhiteSpace(query))
            return new List<UserSearch>();

        // Retrieve users from the user manager based on the search query
        var users = await _userManager.Users
            .Where(u => u.UserName.ToLower().Contains(query.ToLower()) || u.Name.ToLower().Contains(query.ToLower()))
            .Select(u => new UserSearch
            {
                // Create a new UserSearch object with relevant user information
                Id = u.Id,
                UserName = u.UserName,
                Name = u.Name,
                Avatar = u.Avatar
            })
            .ToListAsync();

        // Return the list of users as the result
        return users;
    }

}