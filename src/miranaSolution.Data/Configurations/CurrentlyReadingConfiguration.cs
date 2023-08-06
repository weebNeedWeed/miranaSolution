using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations;

public class CurrentlyReadingConfiguration : IEntityTypeConfiguration<CurrentlyReading>
{
    public void Configure(EntityTypeBuilder<CurrentlyReading> builder)
    {
        builder.ToTable("CurrentlyReadings");
        
        builder.HasKey(x => new
        {
            x.UserId,
            x.BookId
        });

        builder.HasOne(x => x.Book)
            .WithMany(x => x.CurrentlyReadings)
            .HasForeignKey(x => x.BookId);

        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.CurrentlyReadings)
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("getdate()");
    }
}