using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.DTOs.Core.Comments;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Core.Books;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Validations;

namespace miranaSolution.Services.Core.Comments;

public class CommentService : ICommentService
{
    private readonly IUserService _userService;
    private readonly IBookService _bookService;
    private readonly MiranaDbContext _context;
    private readonly IValidatorProvider _validatorProvider;
    
    public CommentService(IUserService userService, IBookService bookService, MiranaDbContext context, IValidatorProvider validatorProvider)
    {
        _userService = userService;
        _bookService = bookService;
        _context = context;
        _validatorProvider = validatorProvider;
    }

    public async Task<CreateBookCommentResponse> CreateBookCommentAsync(CreateBookCommentRequest request)
    {
        _validatorProvider.Validate(request);
        
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

        var comment = new Comment
        {
            BookId = request.BookId,
            UserId = request.UserId,
            Content = request.Content,
            ParentId = request.ParentId
        };

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        var commentVm = MapCommentIntoCommentVm(comment);

        return new CreateBookCommentResponse(commentVm);
    }

    public async Task DeleteBookCommentAsync(DeleteBookCommentRequest request)
    {
        var getBookByIdResponse = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(request.BookId));
        if (getBookByIdResponse.BookVm is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }
        
        var comment = await _context.Comments.FirstOrDefaultAsync(
            x => x.Id == request.CommentId && x.BookId == getBookByIdResponse.BookVm.Id);
        if (comment is null)
        {
            throw new CommentNotFoundException("The comment with given Id does not exist.");
        }

        if (!request.ForceDelete)
        {
            var getUserByIdResponse = await _userService.GetUserByIdAsync(
                new GetUserByIdRequest(request.UserId));
            if (getUserByIdResponse.UserVm is null)
            {
                throw new UserNotFoundException("The user with given Id does not exist.");
            }

            if (!comment.UserId.Equals(getUserByIdResponse.UserVm.Id))
            {
                throw new CommentNotFoundException("The comment with given Id does not exist.");
            }
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<GetBookCommentByIdResponse> GetBookCommentById(GetBookCommentByIdRequest request)
    {
        var getBookByIdResponse = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(request.BookId));
        if (getBookByIdResponse.BookVm is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }
        
        var comment = await _context.Comments.FirstOrDefaultAsync(
            x => x.Id == request.CommentId && x.BookId == getBookByIdResponse.BookVm.Id);
        if (comment is null)
        {
            throw new CommentNotFoundException("The comment with given Id does not exist.");
        }

        var commentVm = MapCommentIntoCommentVm(comment);
        return new GetBookCommentByIdResponse(commentVm);
    }

    public async Task<CountCommentByUserIdResponse> CountCommentByUserIdAsync(CountCommentByUserIdRequest request)
    {
        var getUserByIdResponse = await _userService.GetUserByIdAsync(
            new GetUserByIdRequest(request.UserId));
        if (getUserByIdResponse.UserVm is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }

        var totalComments = await _context.Comments.Where(x => x.UserId == request.UserId).CountAsync();

        return new CountCommentByUserIdResponse(totalComments);
    }

    public async Task<GetAllBookCommentsResponse> GetAllBookCommentsAsync(GetAllBookCommentsRequest request)
    {
        var getBookByIdResponse = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(request.BookId));
        if (getBookByIdResponse.BookVm is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }
        
        var query = _context.Comments
            .Where(x => x.BookId == request.BookId && x.ParentId == request.ParentId);

        if (request.Asc ?? false)
        {
            query = query.OrderBy(x => x.CreatedAt);
        }
        else
        {
            query = query.OrderByDescending(x => x.CreatedAt);
        }
        
        var totalRecords = await query.CountAsync();
        
        var pageIndex = request.PagerRequest.PageIndex;
        var pageSize = request.PagerRequest.PageSize;

        query = query.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        var comments = await query.ToListAsync();
        var commentVms = comments.Select(x => MapCommentIntoCommentVm(x)).ToList();

        var response = new GetAllBookCommentsResponse(
            commentVms,
            new PagerResponse(pageIndex, pageSize, totalRecords));

        return response;
    }

    private CommentVm MapCommentIntoCommentVm(Comment comment)
    {
        var commentVm = new CommentVm(
            comment.Id,
            comment.Content,
            comment.ParentId,
            comment.CreatedAt,
            comment.UpdatedAt,
            comment.UserId,
            comment.BookId);

        return commentVm;
    }
}
