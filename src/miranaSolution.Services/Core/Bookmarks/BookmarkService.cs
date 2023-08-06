using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.DTOs.Core.Bookmarks;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.DTOs.Core.Chapters.Exceptions;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Core.Books;
using miranaSolution.Services.Core.Chapters;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.Services.Core.Bookmarks;

public class BookmarkService : IBookmarkService
{
    private readonly IBookService _bookService;
    private readonly MiranaDbContext _context;
    private readonly IUserService _userService;

    public BookmarkService(IBookService bookService, IUserService userService, MiranaDbContext context,
        IChapterService chapterService)
    {
        _bookService = bookService;
        _userService = userService;
        _context = context;
    }

    public async Task<CreateBookmarkResponse> CreateBookmarkAsync(CreateBookmarkRequest request)
    {
        var getUserByIdResponse = await _userService.GetUserByIdAsync(
            new GetUserByIdRequest(request.UserId));
        if (getUserByIdResponse.UserVm is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var getBookByIdResponse = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(request.BookId));
        if (getBookByIdResponse.BookVm is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        var existChapterWithGivenIndex = await _context.Chapters.AnyAsync(x => x.Index == request.ChapterIndex);
        if (!existChapterWithGivenIndex)
            throw new ChapterNotFoundException("The chapter with given Index does not exist.");

        // Check if the bookmark with the request's UserId and BookId already exists, then deleting it before adding a new one
        var bookmarkWithChapterIndexAlreadyExist = await _context.Bookmarks.AnyAsync(
            x => x.ChapterIndex == request.ChapterIndex
                 && x.BookId == request.BookId
                 && x.UserId == request.UserId);
        if (bookmarkWithChapterIndexAlreadyExist)
            await DeleteBookmarkAsync(
                new DeleteBookmarkRequest(
                    request.UserId,
                    request.BookId));

        var bookmark = new Bookmark
        {
            UserId = request.UserId,
            BookId = request.BookId,
            ChapterIndex = request.ChapterIndex
        };

        await _context.Bookmarks.AddAsync(bookmark);
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
        var getUserByIdResponse = await _userService.GetUserByIdAsync(
            new GetUserByIdRequest(request.UserId));
        if (getUserByIdResponse.UserVm is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var getBookByIdResponse = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(request.BookId));
        if (getBookByIdResponse.BookVm is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

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
        var getUserByIdResponse = await _userService.GetUserByIdAsync(
            new GetUserByIdRequest(request.UserId));
        if (getUserByIdResponse.UserVm is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var query = _context.Bookmarks
            .Where(x => x.UserId == request.UserId);

        if (request.BookId.HasValue)
        {
            var getBookByIdResponse = await _bookService.GetBookByIdAsync(
                new GetBookByIdRequest(request.BookId ?? default));
            if (getBookByIdResponse.BookVm is null)
                throw new BookNotFoundException("The book with given Id does not exist.");

            query = query.Where(x => x.BookId == getBookByIdResponse.BookVm.Id);
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