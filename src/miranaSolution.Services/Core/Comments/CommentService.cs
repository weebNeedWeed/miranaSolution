using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.Comments;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Validations;

namespace miranaSolution.Services.Core.Comments;

public class CommentService : ICommentService
{
    private readonly MiranaDbContext _context;
    private readonly IValidatorProvider _validatorProvider;
    private readonly UserManager<AppUser> _userManager;

    public CommentService(MiranaDbContext context, IValidatorProvider validatorProvider, UserManager<AppUser> userManager)
    {
        _context = context;
        _validatorProvider = validatorProvider;
        _userManager = userManager;
    }

    public async Task<CreateBookCommentResponse> CreateBookCommentAsync(CreateBookCommentRequest request)
    {
        _validatorProvider.Validate(request);

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

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
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        var comment = await _context.Comments.FirstOrDefaultAsync(
            x => x.Id == request.CommentId && x.BookId == book.Id);
        if (comment is null) throw new CommentNotFoundException("The comment with given Id does not exist.");

        if (!request.ForceDelete)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
                throw new UserNotFoundException("The user with given Id does not exist.");

            if (!comment.UserId.Equals(user.Id))
                throw new CommentNotFoundException("The comment with given Id does not exist.");
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<GetBookCommentByIdResponse> GetBookCommentByIdAsync(GetBookCommentByIdRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        var comment = await _context.Comments.FirstOrDefaultAsync(
            x => x.Id == request.CommentId && x.BookId == book.Id);
        if (comment is null) throw new CommentNotFoundException("The comment with given Id does not exist.");

        var commentVm = MapCommentIntoCommentVm(comment);
        return new GetBookCommentByIdResponse(commentVm);
    }

    public async Task<CountCommentByUserIdResponse> CountCommentByUserIdAsync(CountCommentByUserIdRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            throw new UserNotFoundException("The user with given Id does not exist.");

        var totalComments = await _context.Comments.Where(x => x.UserId == request.UserId).CountAsync();

        return new CountCommentByUserIdResponse(totalComments);
    }

    public async Task<GetAllBookCommentsResponse> GetAllBookCommentsAsync(GetAllBookCommentsRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        var query = _context.Comments
            .Where(x => x.BookId == request.BookId && x.ParentId == request.ParentId);

        if (request.Asc ?? false)
            query = query.OrderBy(x => x.CreatedAt);
        else
            query = query.OrderByDescending(x => x.CreatedAt);

        var totalRecords = await query.CountAsync();

        var pageIndex = request.PagerRequest.PageIndex;
        var pageSize = request.PagerRequest.PageSize;

        query = query.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        var comments = await query.ToListAsync();
        var commentVms = comments.Select(MapCommentIntoCommentVm).ToList();

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