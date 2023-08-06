using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AppUsers");

        builder.Property(x => x.ReadBookCount)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.Property(x => x.ReadChapterCount)
            .IsRequired()
            .HasDefaultValue(0);
    }
}