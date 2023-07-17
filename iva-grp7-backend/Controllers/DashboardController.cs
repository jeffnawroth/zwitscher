using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iva_grp7_backend.Controllers;

/// <summary>
///     A controller for the dashboard.
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    ///     Creates a new instance of the <see cref="DashboardController" /> class.
    /// </summary>
    /// <param name="context"> The posts manager </param>
    /// <param name="userManager"> The user manager </param>
    public DashboardController(ApiDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    ///     Returns an array with amount of posts in the last 7 days.
    /// </summary>
    /// <returns>An array with the amount of posts that got created in each weekday in the last 7 days.</returns>
    /// <response code="200">Returns the array of amount of posts in each weekday last 7 days.</response>
    /// <response code="500">If an exception occurs while retrieving the dates of the posts.</response>
    [HttpGet("PostsPerDay")]
    public async Task<int[]> GetPostsPerDay()
    {
        // Array to store the count of posts for each weekday (0 = Sunday, 1 = Monday, ..., 6 = Saturday)
        int[] weekdays = {0, 0, 0, 0, 0, 0, 0};

        // Retrieve posts from the context for the last 7 days
        var posts = await _context.Posts
            .Where(x => x.Date.Date >= DateTime.Today.Date.AddDays(-7) && x.Date.Date < DateTime.Today.Date)
            .GroupBy(x => x.Date.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync();

        // Loop through the posts and update the count for the corresponding weekday
        for (var i = 0; i < posts.Count; i++)
        {
            // Calculate the number of days between the post date and the start of the week
            // (0 = Sunday, 1 = Monday, ..., 6 = Saturday)
            switch ((int)(posts[i].Date.Date - DateTime.Today.Date.AddDays(-7)).TotalDays)
            {
                case 0:
                    weekdays[0] += posts[i].Count; // Sunday
                    break;
                case 1:
                    weekdays[1] += posts[i].Count; // Monday
                    break;
                case 2:
                    weekdays[2] += posts[i].Count; // Tuesday
                    break;
                case 3:
                    weekdays[3] += posts[i].Count; // Wednesday
                    break;
                case 4:
                    weekdays[4] += posts[i].Count; // Thursday
                    break;
                case 5:
                    weekdays[5] += posts[i].Count; // Friday
                    break;
                case 6:
                    weekdays[6] += posts[i].Count; // Saturday
                    break;
            }
        }

        // Return the array with the count of posts for each weekday
        return weekdays;
    }


    /// <summary>
    ///     Returns an array with amount of users created at each month in the last 12 months.
    /// </summary>
    /// <returns>An array with the amount of users that got created in each month the past 12 months.</returns>
    /// <response code="200">Returns the array of amount of users in each month.</response>
    /// <response code="500">If an exception occurs while retrieving the dates of the users.</response>
    [HttpGet("UsersGrowth")]
    public async Task<int[]> getUsersGrowth()
{
    // Array to store the count of users for each month (0 = Current month, 1 = Previous month, ..., 11 = Oldest month)
    int[] months = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    // Get the start of the current month
    var startMonth = DateTime.Today.Date.AddDays(-(DateTime.Today.Date.Day - 1)); // Rounding down to month start

    // Retrieve users from the user manager for the last 12 months
    var users = await _userManager.Users
        .Where(x => x.CreatedAt.Date >= startMonth.AddMonths(-12) && x.CreatedAt.Date < startMonth)
        .GroupBy(x => x.CreatedAt.Date)
        .Select(g => new { Date = g.Key, Count = g.Count() })
        .ToListAsync();

    // Loop through the users and update the count for the corresponding month
    for (var i = 0; i < users.Count; i++)
    {
        // Calculate the number of months between the user creation date and the start of the current month
        // (0 = Current month, 1 = Previous month, ..., 11 = Oldest month)
        switch (((startMonth.Year - users[i].Date.Year) * 12 + startMonth.Month - users[i].Date.Month + 12) % 12)
        {
            case 0:
                months[0] += users[i].Count; // Current month
                break;
            case 1:
                months[11] += users[i].Count; // Previous month
                break;
            case 2:
                months[10] += users[i].Count;
                break;
            case 3:
                months[9] += users[i].Count;
                break;
            case 4:
                months[8] += users[i].Count;
                break;
            case 5:
                months[7] += users[i].Count;
                break;
            case 6:
                months[6] += users[i].Count;
                break;
            case 7:
                months[5] += users[i].Count;
                break;
            case 8:
                months[4] += users[i].Count;
                break;
            case 9:
                months[3] += users[i].Count;
                break;
            case 10:
                months[2] += users[i].Count;
                break;
            case 11:
                months[1] += users[i].Count; // Oldest month
                break;
        }
    }

    // Return the array with the count of users for each month
    return months;
}


    /// <summary>
    ///     Returns an array with amount of active users in the last 12 months.
    /// </summary>
    /// <returns>An array with the amount of users that posted in the last 12 months.</returns>
    /// <response code="200">Returns the array of amount of active users with 12 month deadline.</response>
    /// <response code="500">If an exception occurs while retrieving the dates of the users.</response>
    [HttpGet("ActiveUsers")]
    public async Task<int[]> getActiveUsers()
{
    // Array to store the count of active users for each month (0 = Current month, 1 = Previous month, ..., 11 = Oldest month)
    int[] months = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    // Get the start of the current month
    var startMonth = DateTime.Today.Date.AddDays(-(DateTime.Today.Date.Day - 1)); // Rounding down to month start

    // Retrieve posts from the context for the last 12 months
    var posts = await _context.Posts
        .Where(x => x.Date.Date >= startMonth.AddMonths(-12) && x.Date.Date < startMonth)
        .GroupBy(x => x.UserId)
        .Select(x => new { UserId = x.Key, PostDates = x.Select(x => new { x.Date.Year, x.Date.Month }).Take(1) })
        .ToListAsync();

    // Extract the post dates for each user
    var result = posts
        .SelectMany(post => post.PostDates)
        .Select(postDate => ((startMonth.Year - postDate.Year) * 12 + startMonth.Month - postDate.Month + 12) % 12);

    // Count the active users for each month
    foreach (var value in result)
    {
        switch (value)
        {
            case 0:
                months[0] += 1; // Current month
                break;
            case 1:
                months[11] += 1; // Previous month
                break;
            case 2:
                months[10] += 1;
                break;
            case 3:
                months[9] += 1;
                break;
            case 4:
                months[8] += 1;
                break;
            case 5:
                months[7] += 1;
                break;
            case 6:
                months[6] += 1;
                break;
            case 7:
                months[5] += 1;
                break;
            case 8:
                months[4] += 1;
                break;
            case 9:
                months[3] += 1;
                break;
            case 10:
                months[2] += 1;
                break;
            case 11:
                months[1] += 1; // Oldest month
                break;
        }
    }

    // Return the array with the count of active users for each month
    return months;
}


    /// <summary>
    ///     Returns an array with percentage of users ages.
    /// </summary>
    /// <returns>An array with the percentages of users that gave their birthdate.</returns>
    /// <response code="200">Returns the array of percentages of users ranges of age.</response>
    /// <response code="500">If an exception occurs while retrieving the dates of the users.</response>
    [HttpGet("AgeDistribution")]
    public async Task<int[]> getAgeDistribution()
{
    // Array to store the age distribution percentages for each age group
    int[] ages = {0, 0, 0, 0, 0};

    // Array to store the count of users for each age group
    float[] age_value = {0, 0, 0, 0, 0};

    var age_count = 0; // Total count of users with known birth dates
    var today = DateTime.Today.Year; // Current year

    // Retrieve users from the user manager and group them by birth date
    var users = await _userManager.Users
        .GroupBy(x => x.BirthDate)
        .Select(g => new { Age = g.Key, Count = g.Count() })
        .ToListAsync();

    for (var i = 0; i < users.Count; i++)
    {
        if (users[i].Age != null)
        {
            var parsedDate = DateTime.Parse(users[i].Age).Year;
            var age = today - parsedDate;

            // Assign the count of users to the corresponding age group based on their age
            switch (age)
            {
                case int n when n >= 18 && n <= 24:
                    age_value[0] += users[i].Count;
                    break;
                case int n when n >= 25 && n <= 34:
                    age_value[1] += users[i].Count;
                    break;
                case int n when n >= 35 && n <= 44:
                    age_value[2] += users[i].Count;
                    break;
                case int n when n >= 45 && n <= 54:
                    age_value[3] += users[i].Count;
                    break;
                case int n when n >= 55:
                    age_value[4] += users[i].Count;
                    break;
            }

            age_count += users[i].Count;
        }
    }

    // Calculate the percentage of users in each age group
    for (var i = 0; i < age_value.Length; i++)
    {
        if (age_value[i] != 0)
        {
            Console.WriteLine(age_value[i]);
            ages[i] = (int)(age_value[i] / age_count * 100);
        }
    }

    // Return the array with the age distribution percentages
    return ages;
}



    /// <summary>
    ///     Returns an array with percentages of users gender.
    /// </summary>
    /// <returns>An array with the percentages of users that gave their gender.</returns>
    /// <response code="200">Returns the array of percentages of users gender.</response>
    /// <response code="500">If an exception occurs while retrieving the dates of the users.</response>
    [HttpGet("GenderDistribution")]
    public async Task<int[]> getGenderDistribution()
    {
        // Array to store the gender distribution percentages (0 = Male, 1 = Female, 2 = Diverse)
        int[] gender = {0, 0, 0};

        // Array to store the count of users for each gender
        float[] gender_value = {0, 0, 0};

        var gender_count = 0; // Total count of users with known gender

        // Retrieve users from the user manager, order them by gender, and group them by gender
        var users = await _userManager.Users
            .OrderBy(x => x.Gender)
            .GroupBy(x => x.Gender)
            .Select(g => new { Gender = g.Key, Count = g.Count() })
            .ToListAsync();

        for (var i = 0; i < users.Count; i++)
        {
            if (users[i].Gender != null)
            {
                // Assign the count of users to the corresponding gender group
                switch (users[i].Gender)
                {
                    case Gender.Male:
                        gender_value[0] = users[i].Count; // Male
                        break;
                    case Gender.Female:
                        gender_value[1] = users[i].Count; // Female
                        break;
                    case Gender.Diverse:
                        gender_value[2] = users[i].Count; // Diverse
                        break;
                }

                gender_count += users[i].Count;
            }
        }

        // Calculate the percentage of users in each gender group
        for (var i = 0; i < gender_value.Length; i++)
        {
            if (gender_value[i] != 0)
                gender[i] = (int)(gender_value[i] / gender_count * 100);
        }

        // Return the array with the gender distribution percentages
        return gender;
    }


    /// <summary>
    ///     Returns the amount of new posts today.
    /// </summary>
    /// <returns>The amount of posts that got created today.</returns>
    /// <response code="200">Returns the amount of new posts from today.</response>
    /// <response code="500">If an exception occurs while retrieving the amount of the posts.</response>
    [HttpGet("PostsToday")]
    public async Task<int> getPostsToday()
    {
        var amount = 0; // Variable to store the count of posts today

        // Retrieve posts from the context that were created today
        var posts = await _context.Posts
            .Where(x => x.Date.Date == DateTime.Today.Date)
            .GroupBy(x => x.Date.Date)
            .Select(x => new { Count = x.Count() })
            .ToListAsync();

        foreach (var post in posts)
            amount = post.Count; // Update the count of posts today

        return amount; // Return the count of posts today
    }


    /// <summary>
    ///     Returns the amount of new users today.
    /// </summary>
    /// <returns>The amount of users that got created today.</returns>
    /// <response code="200">Returns the amount of new users from today.</response>
    /// <response code="500">If an exception occurs while retrieving the amount of the users.</response>
    [HttpGet("UsersGrowthToday")]
    public async Task<int> getUsersGrowthToday()
    {
        var amount = 0; // Variable to store the count of new users created today

        // Retrieve users from the user manager who were created today
        var users = await _userManager.Users
            .Where(u => u.CreatedAt.Date == DateTime.Today.Date)
            .GroupBy(u => u.CreatedAt.Date)
            .Select(u => new { Count = u.Count() })
            .ToListAsync();

        foreach (var user in users)
            amount = user.Count; // Update the count of new users created today

        return amount; // Return the count of new users created today
    }


    /// <summary>
    ///     Returns the amount of active users today.
    /// </summary>
    /// <returns>The amount of users that created at least one post today.</returns>
    /// <response code="200">Returns the amount of users that posted something from today.</response>
    /// <response code="500">If an exception occurs while retrieving the amount of the posts.</response>
    [HttpGet("ActiveUsersToday")]
    public async Task<int> getActiveUsersToday()
    {
        var amount = 0; // Variable to store the count of active users today

        // Retrieve posts from the context that were created today
        var posts = await _context.Posts
            .Where(x => x.Date.Date == DateTime.Today.Date)
            .GroupBy(x => x.UserId)
            .Select(x => new { UserId = x.Key, PostDates = x.Select(x => new { x.Date.Year, x.Date.Month }).Take(1) })
            .ToListAsync();

        foreach (var post in posts)
            amount++; // Increment the count for each active user with a post today

        return amount; // Return the count of active users today
    }

}