using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations;

public class BookRatingConfiguration : IEntityTypeConfiguration<BookRating>
{
    public void Configure(EntityTypeBuilder<BookRating> builder)
    {
        builder.ToTable("BookRatings");

        builder.HasKey(x => new { x.UserId, x.BookId });

        builder.HasOne(x => x.Book)
            .WithMany(x => x.BookRatings)
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.BookRatings)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("getdate()");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("getdate()");
    }
}