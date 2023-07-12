using miranaSolution.DTOs.Catalog.Genres;

namespace miranaSolution.Services.Catalog.Genres;

public interface IGenreService
{
    Task<GetAllGenresResponse> GetAllGenresAsync();

    Task<GetGenreByIdResponse> GetGenreByIdAsync(GetGenreByIdRequest request);
}