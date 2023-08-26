using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations;

public class ChapterRelationshipConfiguration : IEntityTypeConfiguration<ChapterRelationship>
{
    public void Configure(EntityTypeBuilder<ChapterRelationship> builder)
    {
        builder.ToTable("ChapterRelationships");

        builder.HasKey(x => new { x.FromId, x.ToId });

        builder.HasIndex(x => x.FromId).IsUnique();
        builder.HasIndex(x => x.ToId).IsUnique();

        builder.HasOne(x => x.To)
            .WithMany()
            .HasForeignKey(x => x.ToId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasOne(x => x.From)
            .WithMany()
            .HasForeignKey(x => x.FromId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}