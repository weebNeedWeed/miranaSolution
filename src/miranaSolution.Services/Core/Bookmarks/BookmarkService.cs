using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Bookmarks;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.Services.Core.Bookmarks;

public class BookmarkService : IBookmarkService
{
    private readonly MiranaDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public BookmarkService(MiranaDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<CreateBookmarkResponse> CreateBookmarkAsync(CreateBookmarkRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        var existChapterWithGivenIndex = await _context.Chapters.AnyAsync(x => x.Index == request.ChapterIndex);
        if (!existChapterWithGivenIndex)
            throw new ChapterNotFoundException("The chapter with given Index does not exist.");

        // Check if the bookmark with the request's UserId and BookId already exists, then deleting it before adding a new one
        Bookmark? bookmark = await _context.Bookmarks.FirstOrDefaultAsync(
            x => x.BookId == request.BookId
                 && x.UserId == request.UserId);
        if (bookmark is not null)
        {
            bookmark.ChapterIndex = request.ChapterIndex;
        }
        else
        {
            bookmark = new Bookmark
            {
                UserId = request.UserId,
                BookId = request.BookId,
                ChapterIndex = request.ChapterIndex
            };
            await _context.Bookmarks.AddAsync(bookmark);
        }
        
        await _context.SaveChangesAsync();

        var bookmarkVm = new BookmarkVm(
            bookmark.UserId,
            bookmark.BookId,
            bookmark.ChapterIndex,
            bookmark.CreatedAt,
            bookmark.UpdatedAt);

        return new CreateBookmarkResponse(bookmarkVm);
    }

    public async Task DeleteBookmarkAsync(DeleteBookmarkRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        if (!await _context.Bookmarks.AnyAsync(x => x.BookId == request.BookId
                                                   && x.UserId.Equals(request.UserId)))
        {
            throw new BookmarkNotFoundException("The bookmark does not exist.");
        }
        
        var bookmark = new Bookmark
        {
            UserId = request.UserId,
            BookId = request.BookId
        };

        _context.Bookmarks.Remove(bookmark);
        await _context.SaveChangesAsync();
    }

    public async Task<GetAllBookmarksByUserIdResponse> GetAllBookmarksByUserIdAsync(
        GetAllBookmarksByUserIdRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new UserNotFoundException("The user with given Id does not exist.");
        
        var query = _context.Bookmarks
            .Where(x => x.UserId == request.UserId);

        if (request.BookId.HasValue)
        {
            var book = await _context.Books.FindAsync(request.BookId);
            if (book is null)
                throw new BookNotFoundException("The book with given Id does not exist.");

            query = query.Where(x => x.BookId == book.Id);
        }

        var bookmarks = await query.ToListAsync();

        var bookmarkVms = bookmarks.Select(
            x => new BookmarkVm(
                x.UserId,
                x.BookId,
                x.ChapterIndex,
                x.CreatedAt,
                x.UpdatedAt
            )).ToList();

        return new GetAllBookmarksByUserIdResponse(bookmarkVms);
    }

    public async Task<GetAllBookmarksByBookIdResponse> GetAllBookmarksByBookIdAsync(
        GetAllBookmarksByBookIdRequest request)
    {
        if (!await _context.Books.AnyAsync(x => x.Id == request.BookId))
            throw new BookNotFoundException("The book with given Id does not exist.");

        var bookmarks = await _context.Bookmarks
            .Where(x => x.BookId == request.BookId).ToListAsync();

        var bookmarkVms = bookmarks
            .Select(x => new BookmarkVm(x.UserId, x.BookId, x.ChapterIndex, x.CreatedAt, x.UpdatedAt))
            .ToList();

        return new GetAllBookmarksByBookIdResponse(bookmarkVms);
    }
}