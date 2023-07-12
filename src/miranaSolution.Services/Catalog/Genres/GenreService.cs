using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Catalog.Genres;

namespace miranaSolution.Services.Catalog.Genres;

public class GenreService : IGenreService
{
    private readonly MiranaDbContext _context;

    public GenreService(MiranaDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetAllGenresResponse> GetAllGenresAsync()
    {
        var genres = await _context.Genres
            .Select(x => new GenreVm(x.Id, x.Name, x.ShortDescription, x.Slug))
            .ToListAsync();

        var response = new GetAllGenresResponse(genres);

        return response;
    }

    public async Task<GetGenreByIdResponse> GetGenreByIdAsync(GetGenreByIdRequest request)
    {
        var genre = await _context.Genres.FindAsync(request.GenreId);
        if (genre is null)
        {
            return new GetGenreByIdResponse(null);
        }

        var response = new GetGenreByIdResponse(
            new GenreVm(
                genre.Id,
                genre.Name,
                genre.ShortDescription,
                genre.Slug));

        return response;
    }
}