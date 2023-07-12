using Microsoft.EntityFrameworkCore;
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

    public async Task<GetAllAuthorsResponse> GetAllAuthorsAsync()
    {
        var authorVms = await _context.Authors
            .Select(x => new AuthorVm(
                x.Id,
                x.Name,
                x.Slug))
            .ToListAsync();

        var response = new GetAllAuthorsResponse(authorVms);

        return response;
    }
}