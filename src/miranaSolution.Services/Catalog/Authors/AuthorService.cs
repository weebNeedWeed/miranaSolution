using AutoMapper;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Catalog.Authors;

namespace miranaSolution.Services.Catalog.Authors;

public class AuthorService : IAuthorService
{
    private readonly MiranaDbContext _context;

    public AuthorService(MiranaDbContext context)
    {
        _context = context;
    }

    public async Task<List<AuthorDto>> GetAll()
    {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<Author, AuthorDto>());
        var mapper = config.CreateMapper();

        var authorList = await _context.Authors
            .Select(x => mapper.Map<AuthorDto>(x)).ToListAsync();

        return authorList;
    }
}