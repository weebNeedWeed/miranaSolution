using miranaSolution.Dtos.Common;

namespace miranaSolution.Dtos.Catalog.Books
{
    public class BookCreateRequest
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool IsRecommended { get; set; }
        public string Slug { get; set; }
        public int AuthorId { get; set; }
        public List<CheckboxItem> Genres { get; set; }
    }
}