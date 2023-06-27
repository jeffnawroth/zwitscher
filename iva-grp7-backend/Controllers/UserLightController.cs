using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace iva_grp7_backend.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserLightController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserLightController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    /// <summary>
    /// Gets a list of all followed users with specified attributes.
    /// </summary>
    /// <returns>A list of all followed users from the current user.</returns>
    /// <response code="200">Returns the list of followed users with the specified attributes.</response>
    /// <response code="500">If an exception occurs while retrieving the users.</response>
    [HttpGet]
    public async Task<IActionResult> GetFollowerdUsersLight()
    {
        //A light-user should only contain: id, avatar, name, username
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentUser = await _userManager.FindByEmailAsync(userEmail);
        var followingIds = new List<string>();
        var followingUsers = new List<UserLight>();

        if (currentUser == null)
        {
            return NotFound("Current user not found.");
        }

        var user = await _userManager.Users
                                        .Include(u => u.Following).ThenInclude(f => f.Following)
                                        .Where(u => u.UserName == currentUser.UserName)
                                        .ToListAsync();

        foreach (var u in user)
        {
            followingIds = u.Following != null
            ? u.Following.Select(f => f.FollowingId).ToList()
            : new List<string>();
        }

        for (int i = 0; i < followingIds.Count; i++)
        {
            var follower = await _userManager.Users
                                .Where(u => u.Id == followingIds[i])
                                .Select(u => new { Id = u.Id, Avatar = u.Avatar, Username = u.UserName, Name = u.Name })
                                .ToListAsync();

            foreach (var follow in follower)
            {
                var followingUser = new UserLight()
                {
                    Id = follow.Id,
                    Avatar = follow.Avatar,
                    Username = follow.Username,
                    Name = follow.Name
                };
                followingUsers.Add(followingUser);
            }
        }
        return Ok(followingUsers);
    }
}


