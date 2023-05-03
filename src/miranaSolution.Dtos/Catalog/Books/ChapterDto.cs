namespace miranaSolution.Dtos.Catalog.Books;

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
    public int BookId { get; set; }
    public string BookName { get; set; }
    public string Genre { get; set; }
    public string BookAuthor { get; set; }
}