using miranaSolution.DTOs.Catalog.Genres;

namespace miranaSolution.Services.Catalog.Genres;

public interface IGenreService
{
    Task<List<GenreDto>> GetAll();

    Task<GenreDto> GetById(int id);
}