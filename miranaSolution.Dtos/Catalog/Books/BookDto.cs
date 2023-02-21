namespace miranaSolution.Dtos.Catalog.Books
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ThumbnailImage { get; set; }
        public bool IsRecommended { get; set; }
        public string Slug { get; set; }
    }
}