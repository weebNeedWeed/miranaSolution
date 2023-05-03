using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Configurations
{
    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.ToTable("BookGenres");

            builder.HasKey(x => new { x.BookId, x.GenreId });

            builder.HasOne(x => x.Genre)
                .WithMany(x => x.BookGenres)
                .HasForeignKey(x => x.GenreId);

            builder.HasOne(x => x.Book)
                .WithMany(x => x.BookGenres)
                .HasForeignKey(x => x.BookId);
        }
    }
}