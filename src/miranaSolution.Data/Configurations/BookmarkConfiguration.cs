using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations;

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.ToTable("Bookmarks");

        builder.HasKey(x => new 
            { 
                x.UserId,
                x.BookId
            });

        builder.HasOne(x => x.Book)
            .WithMany(x => x.Bookmarks)
            .HasForeignKey(x => x.BookId);

        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.Bookmarks)
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("getdate()");
        
        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("getdate()");
    }
}