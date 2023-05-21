namespace miranaSolution.Dtos.Catalog.Books;

public class ChapterCreateRequest
{
    public int Index { get; set; }
    public string Name { get; set; }
    public int WordCount { get; set; }
    public string Content { get; set; }
}