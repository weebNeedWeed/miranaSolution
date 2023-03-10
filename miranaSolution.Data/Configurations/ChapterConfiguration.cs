using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable("Chapters");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.CreatedAt)
                .IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.UpdatedAt)
                .IsRequired().HasDefaultValue(DateTime.Now);

            builder.HasOne(x => x.Book)
                .WithMany(x => x.Chapters)
                .HasForeignKey(x => x.BookId);
        }
    }
}