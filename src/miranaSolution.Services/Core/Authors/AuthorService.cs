using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Authors;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Validations;

namespace miranaSolution.Services.Core.Authors;

public class AuthorService : IAuthorService
{
    private readonly MiranaDbContext _context;
    private readonly IValidatorProvider _validatorProvider;
    
    public AuthorService(MiranaDbContext context, IValidatorProvider validatorProvider)
    {
        _context = context;
        _validatorProvider = validatorProvider;
    }

    public async Task<GetAllAuthorsResponse> GetAllAuthorsAsync()
    {
        var authorVms = await _context.Authors
            .Select(x => new AuthorVm(
                x.Id,
                x.Name))
            .ToListAsync();

        var response = new GetAllAuthorsResponse(authorVms);

        return response;
    }

    public async Task<CreateAuthorResponse> CreateAuthorAsync(CreateAuthorRequest request)
    {
        _validatorProvider.Validate(request);
        
        var author = new Author
        {
            Name = request.Name
        };

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        var authorVm = new AuthorVm(
            author.Id,
            author.Name);

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
        _validatorProvider.Validate(request);
        
        var author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == request.AuthorId);
        if (author is null)
        {
            throw new AuthorNotFoundException("The author with given Id does not exist.");
        }
        
        author.Name = request.Name;

        await _context.SaveChangesAsync();
        
        var authorVm = new AuthorVm(
            author.Id,
            author.Name);

        return new UpdateAuthorResponse(authorVm);
    }

    public async Task<GetAllBooksByAuthorIdResponse> GetAllBookByAuthorIdAsync(GetAllBooksByAuthorIdRequest request)
    {
        if (await _context.Authors.FindAsync(request.AuthorId) is null)
        {
            throw new AuthorNotFoundException("The author with given Id does not exist.");
        }

        var books = await _context.Books
            .Where(x => x.AuthorId == request.AuthorId)
            .OrderByDescending(x => x.UpdatedAt)
            .Take(request.NumberOfBooks)
            .ToListAsync();

        var bookVms = books.Select(x => new BookVm(
            x.Id,
            x.Name,
            x.ShortDescription,
            x.LongDescription,
            x.CreatedAt,
            x.UpdatedAt,
            x.ThumbnailImage,
            x.IsRecommended,
            x.Slug,
            "",
            new List<string>(),
            x.IsDone,
            request.AuthorId)).ToList();

        return new GetAllBooksByAuthorIdResponse(bookVms);
    }
}