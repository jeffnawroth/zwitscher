using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

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
        /// Returns an array with amount of posts in weekday.
        /// </summary>
        /// <returns>An array with the amount of posts that got created in each weekday.</returns>
        /// <response code="200">Returns the array of amount of posts in each weekday.</response>
        /// <response code="500">If an exception occurs while retrieving the dates of the posts.</response>
        [HttpGet("PostsPerDay")]
        public async Task<int[]> GetPostsPerDay()
        {
            //Array for weekdays
            int[] weekdays = {0,0,0,0,0,0,0};

            var posts = await _context.Posts
                .GroupBy(x => x.Date.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();
            
            for(int i = 0; i < posts.Count;i++)
            {
                var day = posts[i].Date.Day;
                var month = posts[i].Date.Month;
                switch (month)
                {
                    case 1: month = 11; break;
                    case 2: month = 12; break;
                    default: month -= 2; break;
                }
                var century = posts[i].Date.Year / 100;
                var year = posts[i].Date.Year % 100;
                
                //Calculating which weekday the post was created on 0 = Sunday ; 1 = Monday ; ... 6 = Saturday; (GREGORIAN CALENDAR ONLY)
                var weekday = (day + (int)(2.6 * month - 0.2) - (2 * century) + year + (int)(year / 4) + (int)(century / 4)) % 7;

                switch(weekday) 
                {
                    case 1: weekdays[0] += posts[i].Count; break;
                    case 2: weekdays[1] += posts[i].Count; break;
                    case 3: weekdays[2] += posts[i].Count; break;
                    case 4: weekdays[3] += posts[i].Count; break;
                    case 5: weekdays[4] += posts[i].Count; break;
                    case 6: weekdays[5] += posts[i].Count; break;
                    case 0: weekdays[6] += posts[i].Count; break;
                }   
            }
            return weekdays;
        }

        /// <summary>
        /// Returns an array with amount of users created at each month.
        /// </summary>
        /// <returns>An array with the amount of users that got created in each month.</returns>
        /// <response code="200">Returns the array of amount of users in each month.</response>
        /// <response code="500">If an exception occurs while retrieving the dates of the users.</response>
        [HttpGet("UsersGrowth")]
        public async Task<int[]> getUsersGrowth()
        {
            //Array for months
            int[] months = {0,0,0,0,0,0,0,0,0,0,0,0};

            var users = await _userManager.Users
                .GroupBy(x => x.CreatedAt.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            for(int i = 0; i < users.Count; i++)
            {
                switch(users[i].Month)
                {
                    case 1: months[0] += users[i].Count; break;
                    case 2: months[1] += users[i].Count; break;
                    case 3: months[2] += users[i].Count; break;
                    case 4: months[3] += users[i].Count; break;
                    case 5: months[4] += users[i].Count; break;
                    case 6: months[5] += users[i].Count; break;
                    case 7: months[6] += users[i].Count; break;
                    case 8: months[7] += users[i].Count; break;
                    case 9: months[8] += users[i].Count; break;
                    case 10: months[9] += users[i].Count; break;
                    case 11: months[10] += users[i].Count; break;
                    case 12: months[11] += users[i].Count; break;
                }
            }

            return months;
        }

        /// <summary>
        /// Returns an array with amount of active users in the last 6 months.
        /// </summary>
        /// <returns>An array with the amount of users that posted in the last 6 months.</returns>
        /// <response code="200">Returns the array of amount of active users with 6 month deadline.</response>
        /// <response code="500">If an exception occurs while retrieving the dates of the users.</response>
        [HttpGet("ActiveUsers")]
        public async Task<int[]> getActiveUsers()
        {
            //Array für Monate
            int[] months = {0,0,0,0,0,0,0,0,0,0,0,0};
            int deadline = 6;

            var posts = await _context.Posts
                .Where(x => x.Date.Month > (DateTime.Today.Month - deadline))
                .GroupBy(x => x.UserId)
                .Select(g => new {Id = g.Key})
                .ToListAsync();

            var users = await _userManager.Users
                .Select(g => new {g.CreatedAt.Month})
                .ToListAsync();
            
            for(int i = 0; i < posts.Count; i++)
            { 
                switch (users[i].Month)
                {
                    case 1: months[0] += 1; break;
                    case 2: months[1] += 1; break;
                    case 3: months[2] += 1; break;
                    case 4: months[3] += 1; break;
                    case 5: months[4] += 1; break;
                    case 6: months[5] += 1; break;
                    case 7: months[6] += 1; break;
                    case 8: months[7] += 1; break;
                    case 9: months[8] += 1; break;
                    case 10: months[9] += 1; break;
                    case 11: months[10] += 1; break;
                    case 12: months[11] += 1; break;
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
    }