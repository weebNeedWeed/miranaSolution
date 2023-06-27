namespace miranaSolution.Dtos.Catalog.Books.Chapters;

public class ChapterDto
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ReadCount { get; set; }
    public int WordCount { get; set; }
    public string Content { get; set; }
    public int TotalRecords { get; set; }
}