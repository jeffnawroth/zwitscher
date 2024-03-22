using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace iva_grp7_backend;

public class ApiDbContext : IdentityDbContext<ApplicationUser>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    // Represents a database table of refresh tokens.
    public DbSet<RefreshToken> RefreshTokens { get; set; } // Database table for storing refresh tokens

    public DbSet<Post> Posts { get; set; } // Database table for storing posts

    public DbSet<UserFollower> UserFollowers { get; set; } // Database table for storing user followers

    public DbSet<UserFollowing> UserFollowings { get; set; } // Database table for storing user followings

    public DbSet<UserInterest> UserInterests { get; set; } // Database table for storing user interests

    public DbSet<PostVote> PostVotes { get; set; } // Database table for storing post votes

    public DbSet<PostFile> PostFiles { get; set; } // Database table for storing post files

    public DbSet<Comment> Comments { get; set; } // Database table for storing comments


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
        .OnDelete(DeleteBehavior.NoAction); // User deletion does not cascade to followers

    modelBuilder.Entity<UserFollower>()
        .HasOne(uf => uf.Follower)
        .WithMany()
        .HasForeignKey(uf => uf.FollowerId)
        .OnDelete(DeleteBehavior.NoAction); // Follower deletion does not cascade

    // UserFollowing configuration
    modelBuilder.Entity<UserFollowing>()
        .HasKey(uf => new { uf.UserId, uf.FollowingId }); // Defining composite key

    modelBuilder.Entity<UserFollowing>()
        .HasOne(uf => uf.User)
        .WithMany(u => u.Following)
        .HasForeignKey(uf => uf.UserId)
        .OnDelete(DeleteBehavior.NoAction); // User deletion does not cascade to following relationships

    modelBuilder.Entity<UserFollowing>()
        .HasOne(uf => uf.Following)
        .WithMany()
        .HasForeignKey(uf => uf.FollowingId)
        .OnDelete(DeleteBehavior.NoAction); // Following deletion does not cascade

    // PostVote configuration
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

    // UserInterest configuration
    modelBuilder.Entity<UserInterest>()
        .HasKey(ui => new { ui.UserId, ui.Interest }); // Defining composite key

    modelBuilder.Entity<UserInterest>()
        .HasOne(ui => ui.User)
        .WithMany(u => u.Interests)
        .HasForeignKey(ui => ui.UserId)
        .OnDelete(DeleteBehavior.NoAction); // User deletion does not cascade to interests

    // Post-File relationship configuration
    modelBuilder.Entity<Post>()
        .HasMany(p => p.Files)
        .WithOne(f => f.Post)
        .HasForeignKey(f => f.PostId);

    // Comment configuration
    modelBuilder.Entity<Comment>().ToTable("Comments")
        .HasOne(c => c.ParentPost)
        .WithMany(p => p.Comments)
        .HasForeignKey(c => c.ParentPostId)
        .OnDelete(DeleteBehavior.NoAction); // Comment deletion does not cascade

    // Other configurations remain the same
}

}