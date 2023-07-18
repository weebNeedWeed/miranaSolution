namespace miranaSolution.API.ViewModels.Genres;

public record ApiUpdateGenreRequest(
    string Name,
    string ShortDescription,
    string Slug);