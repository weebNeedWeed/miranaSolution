using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.BookRatings;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Validations;

namespace miranaSolution.Services.Core.BookRatings;

public class BookRatingService : IBookRatingService
{
    private readonly MiranaDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IValidatorProvider _validatorProvider;

    public BookRatingService(MiranaDbContext context, UserManager<AppUser> userManager, IValidatorProvider validatorProvider)
    {
        _context = context;
        _userManager = userManager;
        _validatorProvider = validatorProvider;
    }

    public async Task<CreateBookRatingResponse> CreateBookRatingAsync(CreateBookRatingRequest request)
    {
        _validatorProvider.Validate(request);
        
        if (await _userManager.FindByIdAsync(request.UserId.ToString()) is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }

        if (await _context.Books.FindAsync(request.BookId) is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }

        var bookRating = new BookRating
        {
            UserId = request.UserId,
            BookId = request.BookId,
            Content = request.Content,
            Star = request.Star
        };

        await _context.BookRatings.AddAsync(bookRating);
        await _context.SaveChangesAsync();

        var bookRatingVm = MapBookRatingIntoBookRatingVm(bookRating);

        var response = new CreateBookRatingResponse(bookRatingVm);

        return response;
    }

    public async Task DeleteBookRatingAsync(DeleteBookRatingRequest request)
    {
        var rating = await _context.BookRatings.FirstOrDefaultAsync(
            x => x.BookId == request.BookId && x.UserId.Equals(request.UserId));
        if (rating is null)
        {
            throw new BookRatingNotFoundException("The rating does not exist.");
        }

        _context.BookRatings.Remove(rating);
        await _context.SaveChangesAsync();
    }

    public async Task<UpdateBookRatingResponse> UpdateBookRatingAsync(UpdateBookRatingRequest request)
    {
        _validatorProvider.Validate(request);
        
        var rating = await _context.BookRatings.FirstOrDefaultAsync(
            x => x.BookId == request.BookId && x.UserId.Equals(request.UserId));
        if (rating is null)
        {
            throw new BookRatingNotFoundException("The rating does not exist.");
        }

        rating.Content = request.Content;
        rating.Star = request.Star;
        
        await _context.SaveChangesAsync();

        var bookRatingVm = MapBookRatingIntoBookRatingVm(rating);

        var response = new UpdateBookRatingResponse(bookRatingVm);

        return response;
    }

    public async Task<GetAllBookRatingsByUserIdResponse> GetAllBookRatingsByUserIdAsync(GetAllBookRatingsByUserIdRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId.ToString()) is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }

        var ratings = await _context.BookRatings
            .Where(x => x.UserId.Equals(request.UserId))
            .ToListAsync();

        var ratingVms = ratings.Select(MapBookRatingIntoBookRatingVm).ToList();

        var response = new GetAllBookRatingsByUserIdResponse(ratingVms);

        return response;
    }

    public async Task<GetAllBookRatingsByBookIdResponse> GetAllBookRatingsByBookIdAsync(GetAllBookRatingsByBookIdRequest request)
    {
        if (await _context.Books.FindAsync(request.BookId) is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }

        var pageIndex = request.PagerRequest.PageIndex;
        var pageSize = request.PagerRequest.PageSize;

        var query = _context.BookRatings
            .Where(x => x.BookId == request.BookId);

        if (request.UserId.HasValue)
        {
            query = query.Where(x => x.UserId.Equals(request.UserId));
        }

        var totalRatings = await query.CountAsync();
        
        query = query.OrderByDescending(x => x.Star)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        var ratings = await query.ToListAsync();
        
        var ratingVms = ratings.Select(MapBookRatingIntoBookRatingVm).ToList();

        var response = new GetAllBookRatingsByBookIdResponse(
            ratingVms,
            new PagerResponse(pageIndex, pageSize, totalRatings));

        return response;
    }

    public async Task<CheckUserIsRatedResponse> CheckUserIsRatedAsync(CheckUserIsRatedRequest request)
    {
        var isRated = await _context.BookRatings
            .AnyAsync(x => x.BookId == request.BookId && x.UserId.Equals(request.UserId));

        return new CheckUserIsRatedResponse(isRated);
    }

    public async Task<GetOverviewResponse> GetOverviewAsync(GetOverviewRequest request)
    {
        if (await _context.Books.FindAsync(request.BookId) is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }

        var ratingsByStar = new Dictionary<int, int>
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 }
        };

        var ratings = await _context.BookRatings
            .Where(x => x.BookId == request.BookId)
            .ToListAsync();

        foreach (var rating in ratings)
        {
            ratingsByStar[rating.Star]++;
        }

        var response = new GetOverviewResponse(ratingsByStar);
        return response;
    }

    private BookRatingVm MapBookRatingIntoBookRatingVm(BookRating bookRating)
    {
        var ratingVm = new BookRatingVm(
            bookRating.UserId,
            bookRating.BookId,
            bookRating.Content,
            bookRating.Star,
            bookRating.CreatedAt,
            bookRating.UpdatedAt);

        return ratingVm;
    }
}