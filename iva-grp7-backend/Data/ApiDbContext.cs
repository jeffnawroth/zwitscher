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
        public DbSet<Interest> Interests { get; set; }
        
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //modelBuilder.Entity<List<int>>().HasNoKey();
            modelBuilder.Entity<User>()
                .HasMany(u => u.Interests)
                .WithMany() // oder .WithOne(), je nach Ihrer Anforderung
                .UsingEntity<Dictionary<string, object>>(
                    "UserInterest",
                    j => j.HasOne<Interest>().WithMany(),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.Property<int>("InterestId");
                        j.HasKey("UserId", "InterestId");
                        j.ToTable("UserInterests");
                    });
            modelBuilder.Entity<User>()
                .HasMany(u => u.DislikedPosts)
                .WithOne(p => p.DislikedByUser)
                .HasForeignKey(p => p.DislikedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.LikedPosts)
                .WithOne(p => p.LikedByUser)
                .HasForeignKey(p => p.LikedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Follower>()
                .HasOne(f => f.User)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowerUserId);

            modelBuilder.Entity<Following>()
                .HasOne(f => f.User)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowingUserId);
            
            modelBuilder.Entity<Post>()
                .HasOne(f => f.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(f => f.UserId);
            
        }
        

    }
}

