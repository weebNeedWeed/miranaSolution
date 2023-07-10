namespace miranaSolution.Data.Entities;

public class Author
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }

    public List<Book> Books { get; set; } = new();
}