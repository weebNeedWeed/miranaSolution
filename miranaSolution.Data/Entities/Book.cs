namespace miranaSolution.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ThumbnailImage { get; set; }
        public string Slug { get; set; }
    }
}