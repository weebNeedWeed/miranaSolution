using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Configurations;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Extensions;

namespace miranaSolution.Data.Main
{
    public class MiranaDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public MiranaDbContext(DbContextOptions<MiranaDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new AppRoleConfiguration());
            builder.ApplyConfiguration(new BookConfiguration());
            builder.ApplyConfiguration(new GenreConfiguration());
            builder.ApplyConfiguration(new BookGenreConfiguration());
            builder.ApplyConfiguration(new ChapterConfiguration());
            builder.ApplyConfiguration(new RatingConfiguration());
            builder.ApplyConfiguration(new SlideConfiguration());
            builder.ApplyConfiguration(new ReactionConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new AuthorConfiguration());

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens");

            builder.Seed();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}