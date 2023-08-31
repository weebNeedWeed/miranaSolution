using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Extensions;

namespace miranaSolution.Data.Main;

public class MiranaDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public MiranaDbContext(DbContextOptions<MiranaDbContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<BookGenre> BookGenres { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Slide> Slides { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<CommentReaction> CommentReactions { get; set; }
    public DbSet<BookUpvote> BookUpvotes { get; set; }
    public DbSet<BookRating> BookRatings { get; set; }
    public DbSet<ChapterRelationship> ChapterRelationships { get; set; }
    
    public DbSet<CurrentlyReading> CurrentlyReadings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var assembly = typeof(MiranaDbContext).Assembly;
        builder.ApplyConfigurationsFromAssembly(assembly);

        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens");

        // builder.Seed();
    }
}