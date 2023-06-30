using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Validations;

namespace iva_grp7_backend.Controllers;

    /// <summary>
    /// A controller for the dashboard.
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
        /// Creates a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="context"> The posts manager </param>
        /// <param name="userManager"> The user manager </param>
        public DashboardController(ApiDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns an array with amount of posts in the last 7 days.
        /// </summary>
        /// <returns>An array with the amount of posts that got created in each weekday in the last 7 days.</returns>
        /// <response code="200">Returns the array of amount of posts in each weekday last 7 days.</response>
        /// <response code="500">If an exception occurs while retrieving the dates of the posts.</response>
        [HttpGet("PostsPerDay")]
        public async Task<int[]> GetPostsPerDay()
        {
            //Array for weekdays
            int[] weekdays = {0,0,0,0,0,0,0};

            var posts = await _context.Posts
                .Where(x => x.Date.Date >= DateTime.Today.Date.AddDays(-7) && x.Date.Date < DateTime.Today.Date)
                .GroupBy(x => x.Date.Date)
                .Select(g => new { Date = g.Key, Count = g.Count()})
                .ToListAsync();
            
            for(int i = 0; i < posts.Count;i++)
            {
                switch((int)(posts[i].Date.Date - DateTime.Today.Date.AddDays(-7)).TotalDays) 
                {
                    case 0: weekdays[0] += posts[i].Count; break;
                    case 1: weekdays[1] += posts[i].Count; break;
                    case 2: weekdays[2] += posts[i].Count; break;
                    case 3: weekdays[3] += posts[i].Count; break;
                    case 4: weekdays[4] += posts[i].Count; break;
                    case 5: weekdays[5] += posts[i].Count; break;
                    case 6: weekdays[6] += posts[i].Count; break;
                }   
            }
            return weekdays;
        }

        /// <summary>
        /// Returns an array with amount of users created at each month in the last 12 months.
        /// </summary>
        /// <returns>An array with the amount of users that got created in each month the past 12 months.</returns>
        /// <response code="200">Returns the array of amount of users in each month.</response>
        /// <response code="500">If an exception occurs while retrieving the dates of the users.</response>
        [HttpGet("UsersGrowth")]
        public async Task<int[]> getUsersGrowth()
        {
            int[] months = {0,0,0,0,0,0,0,0,0,0,0,0};
            var startMonth = DateTime.Today.Date.AddDays(-(DateTime.Today.Date.Day - 1)); //Rounding down to month start

            var users = await _userManager.Users
                .Where(x => x.CreatedAt.Date >= startMonth.AddMonths(-12) && x.CreatedAt.Date < startMonth)
                .GroupBy(x => x.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            for(int i = 0; i < users.Count; i++)
            {
                switch (((startMonth.Year - users[i].Date.Year) * 12 + startMonth.Month - users[i].Date.Month + 12) % 12)
                {
                    case 0: months[0] += users[i].Count; break;
                    case 1: months[11] += users[i].Count; break;
                    case 2: months[10] += users[i].Count; break;
                    case 3: months[9] += users[i].Count; break;
                    case 4: months[8] += users[i].Count; break;
                    case 5: months[7] += users[i].Count; break;
                    case 6: months[6] += users[i].Count; break;
                    case 7: months[5] += users[i].Count; break;
                    case 8: months[4] += users[i].Count; break;
                    case 9: months[3] += users[i].Count; break;
                    case 10: months[2] += users[i].Count; break;
                    case 11: months[1] += users[i].Count; break;
                }
            }
            return months;
        }

        /// <summary>
        /// Returns an array with amount of active users in the last 12 months.
        /// </summary>
        /// <returns>An array with the amount of users that posted in the last 12 months.</returns>
        /// <response code="200">Returns the array of amount of active users with 12 month deadline.</response>
        /// <response code="500">If an exception occurs while retrieving the dates of the users.</response>
        [HttpGet("ActiveUsers")]
        public async Task<int[]> getActiveUsers()
        {
            //Array für Monate
            int[] months = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var startMonth = DateTime.Today.Date.AddDays(-(DateTime.Today.Date.Day - 1)); //Rounding down to month start

            var posts = await _context.Posts
                    .Where(x => x.Date.Date >= startMonth.AddMonths(-12) && x.Date.Date < startMonth)
                    .GroupBy(x => x.UserId)
                    .Select(x => new { UserId = x.Key, PostDates = x.Select(x => new { Year = x.Date.Year, Month = x.Date.Month }).Take(1) })
                    .ToListAsync();

            var result = posts
                            .SelectMany(post => post.PostDates)
                            .Select(postDate => ((startMonth.Year - postDate.Year) * 12 + startMonth.Month - postDate.Month + 12) % 12);


            foreach (var value in result)
            {   
                switch (value)
                {
                    case 0: months[0] += 1; break;
                    case 1: months[11] += 1; break;
                    case 2: months[10] += 1; break;
                    case 3: months[9] += 1; break;
                    case 4: months[8] += 1; break;
                    case 5: months[7] += 1; break;
                    case 6: months[6] += 1; break;
                    case 7: months[5] += 1; break;
                    case 8: months[4] += 1; break;
                    case 9: months[3] += 1; break;
                    case 10: months[2] += 1; break;
                    case 11: months[1] += 1; break;
                }
            } 
            return months;
        }

        /// <summary>
        /// Returns an array with percentage of users ages.
        /// </summary>
        /// <returns>An array with the percentages of users that gave their birthdate.</returns>
        /// <response code="200">Returns the array of percentages of users ranges of age.</response>
        /// <response code="500">If an exception occurs while retrieving the dates of the users.</response>
        [HttpGet("AgeDistribution")]
        public async Task<int[]> getAgeDistribution()
        {
            //labels: ["18-24 Jahre", "25-34 Jahre", "35-44 Jahre", "45-54 Jahre", "55+"],
            int[] ages = {0,0,0,0,0};
            float[] age_value = {0,0,0,0,0};
            int age_count = 0;
            var today = DateTime.Today.Year;

            var users = await _userManager.Users
                .GroupBy(x => x.BirthDate)
                .Select(g => new { Age = g.Key, Count = g.Count() })
                .ToListAsync();

            for(int i = 0; i < users.Count; i++)
            {
                if (users[i].Age != null)
                {
                    var parsedDate = DateTime.Parse(users[i].Age).Year;
                    var age = today - parsedDate;
                    switch(age) 
                    {
                        case int n when (n >= 18 && n <= 24): age_value[0] = users[i].Count; break;
                        case int n when (n >= 25 && n <= 34): age_value[1] = users[i].Count; break;
                        case int n when (n >= 35 && n <= 44): age_value[2] = users[i].Count; break;
                        case int n when (n >= 45 && n <= 54): age_value[3] = users[i].Count; break;
                        case int n when (n >= 55): age_value[4] = users[i].Count; break;
                    }
                    age_count += users[i].Count;
                }
            }

            for(int i = 0; i < age_value.Length; i++)
            {
                if(age_value[i] != 0)
                {
                    ages[i] = (int)((age_value[i] / age_count) * 100); 
                }
            }

            return ages;
        }


        /// <summary>
        /// Returns an array with percentages of users gender.
        /// </summary>
        /// <returns>An array with the percentages of users that gave their gender.</returns>
        /// <response code="200">Returns the array of percentages of users gender.</response>
        /// <response code="500">If an exception occurs while retrieving the dates of the users.</response>
        [HttpGet("GenderDistribution")]
        public async Task<int[]> getGenderDistribution()
        {
            //labels: ["Männlich", "Weiblich", "Divers"],
            int[] gender = {0,0,0};
            float[] gender_value = {0,0,0};
            int gender_count = 0;

            var users = await _userManager.Users
                        .OrderBy(x => x.Gender)
                        .GroupBy(x => x.Gender)
                        .Select(g => new { Gender = g.Key, Count = g.Count() })
                        .ToListAsync();

            for(int i = 0; i < users.Count;i++)
            {
                if (users[i].Gender != null)
                {
                    switch (users[i].Gender)
                    {
                        case Gender.Male: gender_value[0] = users[i].Count; break;
                        case Gender.Female: gender_value[1] = users[i].Count; break;
                        case Gender.Diverse: gender_value[2] = users[i].Count; break;
                    }
                    gender_count += users[i].Count;
                }
            }

            for(int i = 0; i < gender_value.Length; i++)
            {
                if (gender_value[i] != 0)
                {
                    gender[i] = (int)((gender_value[i] / gender_count) * 100);
                }
            }

            return gender;
        }

        /// <summary>
        /// Returns the amount of new posts today.
        /// </summary>
        /// <returns>The amount of posts that got created today.</returns>
        /// <response code="200">Returns the amount of new posts from today.</response>
        /// <response code="500">If an exception occurs while retrieving the amount of the posts.</response>
        [HttpGet("PostsToday")]
        public async Task<int> getPostsToday()
        {
            int amount = 0;

            var posts = await _context.Posts
                                    .Where(x => x.Date.Date == DateTime.Today.Date)
                                    .GroupBy(x => x.Date.Date)
                                    .Select(x => new {Count = x.Count()})
                                    .ToListAsync();
            
            foreach(var post in posts)
            {
                amount = post.Count;
            }

            return amount;
        }

        /// <summary>
        /// Returns the amount of new users today.
        /// </summary>
        /// <returns>The amount of users that got created today.</returns>
        /// <response code="200">Returns the amount of new users from today.</response>
        /// <response code="500">If an exception occurs while retrieving the amount of the users.</response>
        [HttpGet("UsersGrowthToday")]
        public async Task<int> getUsersGrowthToday()
        {
            int amount = 0;

            var users = await _userManager.Users
                                    .Where(u => u.CreatedAt.Date == DateTime.Today.Date)
                                    .GroupBy(u => u.CreatedAt.Date)
                                    .Select(u => new {Count = u.Count()})
                                    .ToListAsync();

            foreach(var user in users)
            {
                amount = user.Count;
            }

            return amount;
        }

        /// <summary>
        /// Returns the amount of active users today.
        /// </summary>
        /// <returns>The amount of users that created at least one post today.</returns>
        /// <response code="200">Returns the amount of users that posted something from today.</response>
        /// <response code="500">If an exception occurs while retrieving the amount of the posts.</response>
        [HttpGet("ActiveUsersToday")]
        public async Task<int> getActiveUsersToday()
        {
            int amount = 0;

            var posts = await _context.Posts
                                        .Where(x => x.Date.Date == DateTime.Today.Date)
                                        .GroupBy(x => x.UserId)
                                        .Select(x => new { UserId = x.Key, PostDates = x.Select(x => new { Year = x.Date.Year, Month = x.Date.Month }).Take(1) })
                                        .ToListAsync();

            foreach(var post in posts)
            {
                amount++;
            }

            return amount;
        }


    
}