using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.BookUpvotes;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Core.Books;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.Services.Core.BookUpvotes;

public class BookUpvoteService : IBookUpvoteService
{
    private readonly MiranaDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public BookUpvoteService(MiranaDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<CreateBookUpvoteResponse> CreateBookUpvoteAsync(CreateBookUpvoteRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        if (await _context.BookUpvotes.AnyAsync(
                x => x.BookId == request.BookId &&
                     x.UserId.Equals(request.UserId)))
        {
            throw new UserAlreadyUpvotesBookException("The user already upvotes the book.");
        }

        var bookUpvote = new BookUpvote
        {
            BookId = request.BookId,
            UserId = request.UserId
        };

        await _context.BookUpvotes.AddAsync(bookUpvote);
        await _context.SaveChangesAsync();

        var response = new CreateBookUpvoteResponse(
            new BookUpvoteVm(request.UserId, request.BookId));
        return response;
    }

    public async Task DeleteBookUpvoteAsync(DeleteBookUpvoteRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        if (!await _context.BookUpvotes.AnyAsync(
                x => x.BookId == request.BookId &&
                     x.UserId.Equals(request.UserId)))
        {
            throw new UserNotUpvotedBookException("The user did not upvote the book.");
        }
        
        var bookUpvote = new BookUpvote
        {
            UserId = request.UserId,
            BookId = request.BookId
        };

        _context.BookUpvotes.Remove(bookUpvote);
        await _context.SaveChangesAsync();
    }

    public async Task<CountBookUpvoteByBookIdResponse> CountBookUpvoteByBookIdAsync(
        CountBookUpvoteByBookIdRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        var totalUpvotes = await _context.BookUpvotes.CountAsync(x => x.BookId == request.BookId);

        return new CountBookUpvoteByBookIdResponse(totalUpvotes);
    }

    public async Task<CountBookUpvoteByUserIdResponse> CountBookUpvoteByUserIdAsync(
        CountBookUpvoteByUserIdRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var totalUpvotes = await _context.BookUpvotes.CountAsync(x => x.UserId.Equals(request.UserId));

        return new CountBookUpvoteByUserIdResponse(totalUpvotes);
    }

    public async Task<GetAllBookUpvotesResponse> GetAllBookUpvotesAsync(GetAllBookUpvotesRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        var query = _context.BookUpvotes
            .Where(x => x.BookId == request.BookId);

        if (request.UserId.HasValue)
            query = query
                .Where(x => x.UserId.Equals(request.UserId));

        var upvotes = await query.ToListAsync();

        var upvoteVms = upvotes
            .Select(x => new BookUpvoteVm(x.UserId, x.BookId))
            .ToList();

        return new GetAllBookUpvotesResponse(upvoteVms);
    }
}