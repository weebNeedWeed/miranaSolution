using miranaSolution.DTOs.Core.Genres;

namespace miranaSolution.Services.Core.Genres;

public interface IGenreService
{
    Task<GetAllGenresResponse> GetAllGenresAsync();

    Task<GetGenreByIdResponse> GetGenreByIdAsync(GetGenreByIdRequest request);

    Task<CreateGenreResponse> CreateGenreAsync(CreateGenreRequest request);

    Task DeleteGenreAsync(DeleteGenreRequest request);

    Task<UpdateGenreResponse> UpdateGenreAsync(UpdateGenreRequest request);
}