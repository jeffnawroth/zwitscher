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

        if (currentUser == null) return NotFound("Current user not found.");

        if (currentUser.Role == null) return NotFound("Current user role not found.");

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

        if (currentUser.Role == Role.Admin || currentUser.Role == Role.Moderator)
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
        var user = await _userManager.Users
            .Include(u => u.Followers).ThenInclude(f => f.Follower)
            .Include(u => u.Following).ThenInclude(f => f.Following)
            .Include(u => u.Interests)
            .FirstOrDefaultAsync(u => u.Id == id);

        // If no user is found with the given ID, return a 404 Not Found response
        if (user == null) return NotFound();

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
        var user = await _userManager.Users
            .Include(u => u.Followers).ThenInclude(f => f.Follower)
            .Include(u => u.Following).ThenInclude(f => f.Following)
            .Include(u => u.Interests)
            .FirstOrDefaultAsync(u => u.UserName == username);

        // If no user is found with the given username, return a 404 Not Found response
        if (user == null) return NotFound();

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
        // Überprüfen Sie, ob der Modellzustand gültig ist
        if (!ModelState.IsValid)
            // Wenn der Modellzustand ungültig ist, geben Sie eine BadRequest-Antwort mit den Validierungsfehlern zurück
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


        // Rest der Nutzerinformationen
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
            // Holen Sie die Fehler aus dem Ergebnis
            var errors = is_created.Errors.Select(e => e.Description);
            // Geben Sie eine BadRequest-Antwort mit den Fehlern zurück
            return BadRequest(errors);
        }

        if (is_created.Succeeded)
        {
            // Wenn der Benutzer Interessen hat
            if (user.Interests != null)
            {
                // Initialisieren Sie eine Liste, um die UserInterests zu speichern
                var userInterests = new List<UserInterest>();

                foreach (var interestName in user.Interests)
                {
                    // Erstellen Sie ein neues UserInterest, das den Benutzer und das Interesse verknüpft
                    var userInterest = new UserInterest
                    {
                        Interest = interestName,
                        User = applicationUser
                    };

                    // Fügen Sie das UserInterest der Liste hinzu
                    userInterests.Add(userInterest);
                }

                // Weisen Sie die Liste der UserInterests den Interessen des Benutzers zu
                applicationUser.Interests = userInterests;

                // Aktualisieren Sie den Benutzer in der Datenbank
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
                    .Interests // Benutzen Sie die Interessen aus der Eingabe, da sie die gleichen sind wie in der Datenbank
            };

            // Wenn die Benutzererstellung erfolgreich war, geben Sie eine erstellte Antwort mit dem neu erstellten Benutzerobjekt zurück
            return CreatedAtAction(nameof(GetById), new {id = filteredUser.Id}, filteredUser);
        }

        // Wenn die Benutzererstellung nicht erfolgreich war, geben Sie eine BadRequest-Antwort mit den Fehlermeldungen zurück
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
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = await _userManager.FindByEmailAsync(userEmail);

        if (currentUser == null) return NotFound("User not found");
        try
        {
            await _userManager.SetEmailAsync(currentUser, new_email);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_userManager.Users.Any(u => u.Id == currentUser.Id))
                return NotFound();
            throw;
        }

        return Ok("Email wurde aktualisiert");
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
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = await _userManager.FindByEmailAsync(userEmail);

        if (currentUser == null) return NotFound("User not found");
        try
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(currentUser);
            var change = await _userManager.ResetPasswordAsync(currentUser, token, new_password);
            if (!change.Succeeded) return BadRequest("Password konnte nicht geändert werden");
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_userManager.Users.Any(u => u.Id == currentUser.Id))
                return NotFound();
            throw;
        }

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
        if (id != user.Id) return BadRequest();

        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existingUser = await _userManager.FindByIdAsync(id);

        if (existingUser == null) return NotFound();

        if (user.Avatar != null)
        {
            try
            {
                byte[] avatarBytes;

                // Check if the Base64 string contains a comma (and therefore a media type prefix)
                if (user.Avatar.Contains(','))
                {
                    // If the Base64 string contains a comma, split it and only use the second part
                    var fileSplit = user.Avatar.Split(',');
                    try
                    {
                        avatarBytes = Convert.FromBase64String(fileSplit[1]);
                    }
                    catch (FormatException)
                    {
                        return BadRequest("Invalid Avatar format. Please provide a valid Base64 string.");
                    }
                }
                else
                {
                    // If the Base64 string does not contain a comma, use the whole string
                    try
                    {
                        avatarBytes = Convert.FromBase64String(user.Avatar);
                    }
                    catch (FormatException)
                    {
                        return BadRequest("Invalid Avatar format. Please provide a valid Base64 string.");
                    }
                }

                if (existingUser.Avatar == null || !existingUser.Avatar.SequenceEqual(avatarBytes))
                {
                    existingUser.Avatar = avatarBytes;
                }
            }
            catch (Exception e)
            {
                return BadRequest("Fehler beim Aktualisieren des Avatars: " + e.Message);
            }
        }



        existingUser.UserName = user.Username;
        existingUser.Email = user.Email;
        existingUser.Bio = user.Bio;
        existingUser.Gender = user.Gender;
        existingUser.Locked = user.Locked;
        existingUser.Name = user.Name;
        existingUser.BirthDate = user.BirthDate;
        existingUser.Role = user.Role;

        if (user.Interests != null)
        {
            var existingInterests = _context.UserInterests.Where(ui => ui.UserId == existingUser.Id);
            _context.UserInterests.RemoveRange(existingInterests);

            var userInterests = new List<UserInterest>();

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

        var result = await _userManager.UpdateAsync(existingUser);

        if (result.Succeeded)
        {
            await _context.SaveChangesAsync();
            return Ok("User wurde erfolgreich aktualisiert");
        }

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
        var user = await _userManager.FindByIdAsync(id);

        if (user == null) return NotFound();

        var userFollowers = await _context.UserFollowers.Where(uf => uf.UserId == id).ToListAsync();
        _context.UserFollowers.RemoveRange(userFollowers);

        var followedByUsers = await _context.UserFollowers.Where(uf => uf.FollowerId == id).ToListAsync();
        _context.UserFollowers.RemoveRange(followedByUsers);

        var userFollowingUsers = await _context.UserFollowings.Where(uf => uf.UserId == id).ToListAsync();
        _context.UserFollowings.RemoveRange(userFollowingUsers);

        var userFollowedUsers = await _context.UserFollowings.Where(uf => uf.FollowingId == id).ToListAsync();
        _context.UserFollowings.RemoveRange(userFollowedUsers);

        var userInterests = await _context.UserInterests.Where(ui => ui.UserId == id).ToListAsync();
        _context.UserInterests.RemoveRange(userInterests);
        
        // Remove PostVotes related to the user
        var userPostVotes = await _context.PostVotes.Where(pv => pv.UserId == id).ToListAsync();
        _context.PostVotes.RemoveRange(userPostVotes);

        await _context.SaveChangesAsync();

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded) return Ok("Der User wurde erfolgreich gelöscht");

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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = await _userManager.FindByEmailAsync(userId);

        if (currentUser == null) return NotFound("Current user not found.");

        var userToFollow = await _userManager.FindByIdAsync(id);
        if (userToFollow == null) return NotFound();

        var userFollowing = new UserFollowing
        {
            UserId = currentUser.Id,
            FollowingId = userToFollow.Id
        };
        _context.UserFollowings.Add(userFollowing);

        var userFollower = new UserFollower
        {
            UserId = userToFollow.Id,
            FollowerId = currentUser.Id
        };
        _context.UserFollowers.Add(userFollower);

        await _context.SaveChangesAsync();

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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = await _userManager.FindByEmailAsync(userId);

        if (currentUser == null) return NotFound("Current user not found.");

        var userToUnfollow = await _userManager.FindByIdAsync(id);
        if (userToUnfollow == null) return NotFound();

        var userFollowing = await _context.UserFollowings.FirstOrDefaultAsync(
            uf => uf.UserId == currentUser.Id && uf.FollowingId == userToUnfollow.Id
        );

        var userFollower = await _context.UserFollowers.FirstOrDefaultAsync(
            uf => uf.UserId == userToUnfollow.Id && uf.FollowerId == currentUser.Id
        );

        if (userFollowing != null) _context.UserFollowings.Remove(userFollowing);

        if (userFollower != null) _context.UserFollowers.Remove(userFollower);

        await _context.SaveChangesAsync();

        return Ok("Dem User wurde nun erfolgreich entfolgt");
    }

    private async Task<byte[]> ProcessFormFile(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
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
        //A light-user should only contain: id, avatar, name, username
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = await _userManager.FindByEmailAsync(userEmail);
        var followingUsers = new List<UserLight>();

        if (currentUser == null) return NotFound("Current user not found.");

        var followingIds = await _context.UserFollowings
            .Where(f => f.UserId == currentUser.Id)
            .Select(f => f.FollowingId)
            .ToListAsync();

        var follower = await _userManager.Users
            .Where(u => followingIds.Contains(u.Id))
            .ToListAsync();

        foreach (var user in follower)
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

        return followingUsers;
    }
}