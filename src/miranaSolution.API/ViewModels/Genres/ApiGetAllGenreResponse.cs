using miranaSolution.DTOs.Core.Genres;

namespace miranaSolution.API.ViewModels.Genres;

public record ApiGetAllGenreResponse(List<GenreVm> Genres);