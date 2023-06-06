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
        public DbSet<Post> Posts { get; set; }
        public DbSet<Following> Following { get; set; }
        public DbSet<Interest> Interests { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().Ignore(c => c.AccessFailedCount)
                .Ignore(c => c.LockoutEnabled)
                .Ignore(c => c.LockoutEnd)
                .Ignore(c => c.NormalizedUserName)
                .Ignore(c => c.NormalizedEmail)
                .Ignore(c => c.EmailConfirmed)
                .Ignore(c => c.SecurityStamp)
                .Ignore(c => c.ConcurrencyStamp)
                .Ignore(c => c.PhoneNumber)
                .Ignore(c => c.PhoneNumberConfirmed)
                .Ignore(c => c.TwoFactorEnabled)
                .Ignore(c => c.AccessFailedCount);

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");//to change the name of table.
            */
            
            //modelBuilder.Entity<List<int>>().HasNoKey();
            /*
            modelBuilder.Entity<Follower>()
                .HasOne(f => f.User)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowerUserId);

            modelBuilder.Entity<Following>()
                .HasOne(f => f.User)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowingUserId);

            modelBuilder.Entity<Post>()
                .ToTable("Posts")
                .HasOne(f => f.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(f => f.UserId);
            */
        }


    }
}

