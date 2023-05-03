using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("Ratings");

            builder.HasKey(x => new { x.UserId, x.BookId });

            builder.Property(x => x.ParentId).IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .IsRequired().HasDefaultValueSql("getdate()");
            builder.Property(x => x.UpdatedAt)
                .IsRequired().HasDefaultValueSql("getdate()");

            builder.HasOne(x => x.Book)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.BookId);

            builder.HasOne(x => x.AppUser)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.UserId);
        }
    }
}