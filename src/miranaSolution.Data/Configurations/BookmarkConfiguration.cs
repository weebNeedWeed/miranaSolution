using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations;

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.ToTable("Bookmarks");
        
        builder.HasKey(x => new { x.UserId, x.ChapterId });

        builder.HasOne(x => x.Chapter)
            .WithMany(x => x.Bookmarks)
            .HasForeignKey(x => x.ChapterId);

        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.Bookmarks)
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("getdate()");
    }
}