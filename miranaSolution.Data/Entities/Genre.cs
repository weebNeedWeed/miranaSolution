namespace miranaSolution.Data.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Slug { get; set; }

        public List<BookGenre> BookGenres { get; set; }
    }
}