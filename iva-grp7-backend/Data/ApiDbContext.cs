using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace iva_grp7_backend
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
	{
        // Represents a database table of refresh tokens.
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserFollower> UserFollowers { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<PostVote> PostVotes { get; set; }
        public DbSet<PostFile> PostFiles { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensures identity-related configurations are applied
        
            // UserFollower configuration
            modelBuilder.Entity<UserFollower>()
                .HasKey(uf => new { uf.UserId, uf.FollowerId }); // Defining composite key
        
            modelBuilder.Entity<UserFollower>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        
            modelBuilder.Entity<UserFollower>()
                .HasOne(uf => uf.Follower)
                .WithMany()
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.NoAction);
        
            // UserFollowing configuration
            modelBuilder.Entity<UserFollowing>()
                .HasKey(uf => new { uf.UserId, uf.FollowingId }); // Defining composite key
        
            modelBuilder.Entity<UserFollowing>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.Following)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        
            modelBuilder.Entity<UserFollowing>()
                .HasOne(uf => uf.Following)
                .WithMany()
                .HasForeignKey(uf => uf.FollowingId)
                .OnDelete(DeleteBehavior.NoAction);
        
            // Other configurations remain the same
            modelBuilder.Entity<PostVote>()
                .HasKey(pv => new { pv.PostId, pv.UserId }); // Defining composite key
        
            modelBuilder.Entity<PostVote>()
                .HasOne(pv => pv.Post)
                .WithMany(p => p.Votes)
                .HasForeignKey(pv => pv.PostId);
        
            modelBuilder.Entity<PostVote>()
                .HasOne(pv => pv.User)
                .WithMany()
                .HasForeignKey(pv => pv.UserId)
                .OnDelete(DeleteBehavior.NoAction); // To prevent cyclical delete cascade
        
            modelBuilder.Entity<PostFile>()
                .HasOne(pf => pf.Post)
                .WithMany(p => p.Files)
                .HasForeignKey(pf => pf.PostId);
        
            modelBuilder.Entity<UserInterest>()
                .HasKey(ui => new { ui.UserId, ui.Interest }); // Defining composite key
        
            modelBuilder.Entity<UserInterest>()
                .HasOne(ui => ui.User)
                .WithMany(u => u.Interests)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.NoAction); 
        }









    }
}

