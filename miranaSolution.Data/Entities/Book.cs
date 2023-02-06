namespace miranaSolution.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ThumbnailImage { get; set; }
        public string Slug { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }

        public List<Chapter> Chapters { get; set; }

        public List<BookGenre> BookGenres { get; set; }

        public List<Rating> Ratings { get; set; }
    }
}