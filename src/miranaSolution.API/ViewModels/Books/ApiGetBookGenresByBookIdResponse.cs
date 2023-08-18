using miranaSolution.DTOs.Core.Genres;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetBookGenresByBookIdResponse(List<GenreVm> Genres);