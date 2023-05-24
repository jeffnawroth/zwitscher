using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace iva_grp7_backend
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
	{
        // Represents a database table of users.
        //public DbSet<User> Users { get; set; }
        // Represents a database table of posts.
        //public DbSet<Post> Posts { get; set; }
        
        // Represents a database table of refresh tokens.
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        
        

    }
}

