using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations;

public class BookUpvoteConfiguration : IEntityTypeConfiguration<BookUpvote>
{
    public void Configure(EntityTypeBuilder<BookUpvote> builder)
    {
        builder.ToTable("BookUpvotes");

        builder.HasKey(x => new
        {
            x.UserId,
            x.BookId
        });

        builder.HasOne(x => x.Book)
            .WithMany(x => x.BookUpvotes)
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.BookUpvotes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}