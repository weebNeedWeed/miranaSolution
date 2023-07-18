namespace miranaSolution.DTOs.Core.Genres;

public record UpdateGenreRequest(
    int GenreId,
    string Name);