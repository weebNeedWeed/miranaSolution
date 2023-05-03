using miranaSolution.Dtos.Catalog.Genres;

namespace miranaSolution.Business.Catalog.Genres
{
    public interface IGenreService
    {
        Task<List<GenreDto>> GetAll();

        Task<GenreDto> GetById(int id);
    }
}