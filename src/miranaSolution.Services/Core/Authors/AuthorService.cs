using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Authors;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.Services.Core.Authors;

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

    public async Task<CreateAuthorResponse> CreateAuthorAsync(CreateAuthorRequest request)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(x => x.Slug == request.Slug);
        if (author is not null)
        {
            throw new AuthorAlreadyExistsException("The author with given Slug already exists.");
        }

        author = new Author
        {
            Name = request.Name,
            Slug = request.Slug
        };

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        var authorVm = new AuthorVm(
            author.Id,
            author.Name,
            author.Slug);

        return new CreateAuthorResponse(authorVm);
    }

    public async Task DeleteAuthorAsync(DeleteAuthorRequest request)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == request.AuthorId);
        if (author is null)
        {
            throw new AuthorNotFoundException("The author with given Id does not exist.");
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }

    public async Task<UpdateAuthorResponse> UpdateAuthorAsync(UpdateAuthorRequest request)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == request.AuthorId);
        if (author is null)
        {
            throw new AuthorNotFoundException("The author with given Id does not exist.");
        }

        author.Slug = request.Slug;
        author.Name = request.Name;

        await _context.SaveChangesAsync();
        
        var authorVm = new AuthorVm(
            author.Id,
            author.Name,
            author.Slug);

        return new UpdateAuthorResponse(authorVm);
    }
}