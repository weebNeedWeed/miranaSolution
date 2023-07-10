using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations;

public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.ToTable("Chapters");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.CreatedAt)
            .IsRequired().HasDefaultValueSql("getdate()");
        ;
        builder.Property(x => x.UpdatedAt)
            .IsRequired().HasDefaultValueSql("getdate()");

        builder.HasIndex(x => new { x.BookId, x.Index }).IsUnique();

        builder.HasOne(x => x.Book)
            .WithMany(x => x.Chapters)
            .HasForeignKey(x => x.BookId);
    }
}