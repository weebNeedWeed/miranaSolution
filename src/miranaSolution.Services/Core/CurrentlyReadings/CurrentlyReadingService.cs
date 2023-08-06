using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Chapters.Exceptions;
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

    public async Task<AddBookResponse> AddBookAsync(AddBookRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId.ToString()) is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        if (await _context.Books.FindAsync(request.BookId) is null)
            throw new BookNotFoundException("The book with given Id does not exist.");
        
        var existChapterWithGivenIndex = await _context.Chapters.AnyAsync(x => x.Index == request.ChapterIndex);
        if (!existChapterWithGivenIndex)
            throw new ChapterNotFoundException("The chapter with given Index does not exist.");

        var isBookAddedToCurrentlyReadings = await _context.CurrentlyReadings
            .AnyAsync(
                x => x.ChapterIndex == request.ChapterIndex &&
                     x.UserId.Equals(request.UserId) &&
                     x.BookId == request.BookId);
        if (isBookAddedToCurrentlyReadings)
        {
            await RemoveBookAsync(
                new RemoveBookRequest(
                    request.UserId, request.BookId));
        }

        var currentlyReadingBook = new CurrentlyReading
        {
            ChapterIndex = request.ChapterIndex,
            UserId = request.UserId,
            BookId = request.BookId
        };

        await _context.CurrentlyReadings.AddAsync(currentlyReadingBook);
        await _context.SaveChangesAsync();

        var currentlyReadingVm = new CurrentlyReadingVm(
            currentlyReadingBook.BookId,
            currentlyReadingBook.UserId,
            currentlyReadingBook.ChapterIndex,
            currentlyReadingBook.CreatedAt);

        return new AddBookResponse(currentlyReadingVm);
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

        var currReadings = await query.OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        var currReadingVms = currReadings.Select(x => new CurrentlyReadingVm(
            x.BookId,
            x.UserId,
            x.ChapterIndex,
            x.CreatedAt)).ToList();

        return new GetCurrentlyReadingBooksResponse(currReadingVms);
    }
}