using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.DTOs.Core.BookUpvotes;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Core.Books;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.Services.Core.BookUpvotes;

public class BookUpvoteService : IBookUpvoteService
{
    private readonly IUserService _userService;
    private readonly IBookService _bookService;
    private readonly MiranaDbContext _context;

    public BookUpvoteService(IUserService userService, MiranaDbContext context, IBookService bookService)
    {
        _userService = userService;
        _context = context;
        _bookService = bookService;
    }
    
    public async Task<CreateBookUpvoteResponse> CreateBookUpvoteAsync(CreateBookUpvoteRequest request)
    {
        var getUserByIdResponse = await _userService.GetUserByIdAsync(
            new GetUserByIdRequest(request.UserId));
        if (getUserByIdResponse.UserVm is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }

        var getBookByIdResponse = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(request.BookId));
        if (getBookByIdResponse.BookVm is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
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
        var getUserByIdResponse = await _userService.GetUserByIdAsync(
            new GetUserByIdRequest(request.UserId));
        if (getUserByIdResponse.UserVm is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }
        
        var getBookByIdResponse = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(request.BookId));
        if (getBookByIdResponse.BookVm is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }

        var bookUpvote = new BookUpvote
        {
            UserId = request.UserId,
            BookId = request.BookId
        };

        _context.BookUpvotes.Remove(bookUpvote);
        await _context.SaveChangesAsync();
    }

    public async Task<CountBookUpvoteByBookIdResponse> CountBookUpvoteByBookIdAsync(CountBookUpvoteByBookIdRequest request)
    {
        var getBookByIdResponse = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(request.BookId));
        if (getBookByIdResponse.BookVm is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }

        var totalUpvotes = await _context.BookUpvotes.CountAsync(x => x.BookId == request.BookId);

        return new CountBookUpvoteByBookIdResponse(totalUpvotes);
    }

    public async Task<CountBookUpvoteByUserIdResponse> CountBookUpvoteByUserIdAsync(CountBookUpvoteByUserIdRequest request)
    {
        var getUserByIdResponse = await _userService.GetUserByIdAsync(
            new GetUserByIdRequest(request.UserId));
        if (getUserByIdResponse.UserVm is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }
        
        var totalUpvotes = await _context.CommentReactions.CountAsync(x => x.UserId == request.UserId);

        return new CountBookUpvoteByUserIdResponse(totalUpvotes);
    }
}