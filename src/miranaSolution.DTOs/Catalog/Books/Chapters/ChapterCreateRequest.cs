namespace miranaSolution.DTOs.Catalog.Books.Chapters;

public class ChapterCreateRequest
{
    public string Name { get; set; }
    public int WordCount { get; set; }
    public string Content { get; set; }
}