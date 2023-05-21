using AutoMapper;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.Dtos.Catalog.Genres;

namespace miranaSolution.Business.Catalog.Genres
{
    public class GenreService : IGenreService
    {
        private readonly MiranaDbContext _context;

        public GenreService(MiranaDbContext context)
        {
            _context = context;
        }

        public async Task<List<GenreDto>> GetAll()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Genre, GenreDto>());
            var mapper = config.CreateMapper();

            var data = await _context.Genres.Select(x => mapper.Map<GenreDto>(x)).ToListAsync();

            return data;
        }

        public Task<GenreDto> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}