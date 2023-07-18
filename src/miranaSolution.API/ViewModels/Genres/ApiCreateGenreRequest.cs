namespace miranaSolution.API.ViewModels.Genres;

public record ApiCreateGenreRequest(
    string Name,
    string ShortDescription,
    string Slug);