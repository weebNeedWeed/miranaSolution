namespace miranaSolution.Data.Entities;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public List<BookGenre> BookGenres { get; set; } = new();
}