using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Catalog.Books;

public class BookUpdateRequest
{
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }
    public bool IsRecommended { get; set; }
    public string Slug { get; set; }
    public int AuthorId { get; set; }
    public List<CheckboxItem> Genres { get; set; }
}