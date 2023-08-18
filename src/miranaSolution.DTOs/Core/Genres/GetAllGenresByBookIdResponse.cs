namespace miranaSolution.DTOs.Core.Genres;

public record GetAllGenresByBookIdResponse(
    List<GenreVm> GenreVms);