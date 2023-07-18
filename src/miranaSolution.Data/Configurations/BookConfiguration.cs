using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).UseIdentityColumn();

        builder.Property(x => x.CreatedAt)
            .IsRequired().HasDefaultValueSql("getdate()");
        builder.Property(x => x.UpdatedAt)
            .IsRequired().HasDefaultValueSql("getdate()");
        builder.Property(x => x.IsRecommended)
            .IsRequired().HasDefaultValue(false);
        builder.Property(x => x.IsDone)
            .IsRequired().HasDefaultValue(false);

        builder.Property(x => x.Slug).IsUnicode().IsRequired();

        builder.HasIndex(x => x.Slug).IsUnique();

        builder.HasOne(x => x.Author)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}