using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.CurrentlyReading;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.Services.Core.CurrentlyReadings;

public class CurrentlyReadingService : ICurrentlyReadingService
{
    private readonly MiranaDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public CurrentlyReadingService(MiranaDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task AddBookAsync(AddBookRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId.ToString()) is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        if (await _context.Books.FindAsync(request.BookId) is null)
            throw new BookNotFoundException("The book with given Id does not exist.");
        
        var existChapterWithGivenIndex = await _context.Chapters.AnyAsync(x => x.Index == request.ChapterIndex);
        if (!existChapterWithGivenIndex)
            throw new ChapterNotFoundException("The chapter with given Index does not exist.");

        var curr = await _context.CurrentlyReadings
            .FirstOrDefaultAsync(x => x.UserId.Equals(request.UserId) &&
                                      x.BookId == request.BookId);
        if (curr is not null)
        {
            curr.ChapterIndex = request.ChapterIndex;
        }
        else
        {
            curr = new CurrentlyReading
            {
                ChapterIndex = request.ChapterIndex,
                UserId = request.UserId,
                BookId = request.BookId
            };
            await _context.CurrentlyReadings.AddAsync(curr);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task RemoveBookAsync(RemoveBookRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId.ToString()) is null)
            throw new UserNotFoundException("The user with given Id does not exist.");
        
        if (await _context.Books.FindAsync(request.BookId) is null)
            throw new BookNotFoundException("The book with given Id does not exist.");
        
        var currentlyReadingBook = new CurrentlyReading
        {
            UserId = request.UserId,
            BookId = request.BookId
        };

        _context.CurrentlyReadings.Remove(currentlyReadingBook);
        await _context.SaveChangesAsync();
    }

    public async Task<GetCurrentlyReadingBooksResponse> GetCurrentlyReadingBooksAsync(GetCurrentlyReadingBooksRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId.ToString()) is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var query = _context.CurrentlyReadings
            .Where(x => x.UserId.Equals(request.UserId));

        if (request.BookId.HasValue)
        {
            if (await _context.Books.FindAsync(request.BookId) is null)
                throw new BookNotFoundException("The book with given Id does not exist.");

            query = query.Where(x => x.BookId == request.BookId);
        }

        var newQuery = from curr in query
            join book in _context.Books on curr.BookId equals book.Id
            orderby curr.CreatedAt descending 
            select new
            {
                curr,
                book,
            };

        var items = await newQuery.ToListAsync();

        var currReadingVms = items.Select(x => new CurrentlyReadingVm(
            x.book.Id,
            x.book.Name,
            x.book.ThumbnailImage,
            x.book.Slug,
            x.curr.UserId,
            x.curr.ChapterIndex,
            x.curr.CreatedAt)).ToList();

        return new GetCurrentlyReadingBooksResponse(currReadingVms);
    }
}