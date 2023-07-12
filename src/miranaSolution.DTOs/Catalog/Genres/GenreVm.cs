namespace miranaSolution.DTOs.Catalog.Genres;

public record GenreVm(
    int Id,
    string Name,
    string ShortDescription,
    string Slug);