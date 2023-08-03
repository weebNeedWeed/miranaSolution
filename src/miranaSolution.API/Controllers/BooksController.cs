using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Books;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.DTOs.Core.BookUpvotes;
using miranaSolution.DTOs.Core.Chapters;
using miranaSolution.DTOs.Core.Comments;
using miranaSolution.Services.Core.Books;
using miranaSolution.Services.Core.BookUpvotes;
using miranaSolution.Services.Core.Chapters;
using miranaSolution.Services.Core.Comments;
using miranaSolution.Services.Exceptions;
using miranaSolution.Utilities.Constants;
using miranaSolution.API.Extensions;
using miranaSolution.DTOs.Core.Bookmarks;
using miranaSolution.DTOs.Core.BookRatings;
using miranaSolution.Services.Core.Bookmarks;
using miranaSolution.Services.Core.BookRatings;

namespace miranaSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RolesConstant.Administrator)]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IBookmarkService _bookmarkService;
    private readonly IChapterService _chapterService;
    private readonly ICommentService _commentService;
    private readonly IBookUpvoteService _bookUpvoteService;
    private readonly IBookRatingService _bookRatingService;

    public BooksController(IBookService bookService, IChapterService chapterService, ICommentService commentService,
        IBookUpvoteService bookUpvoteService, IBookRatingService bookRatingService, IBookmarkService bookmarkService)
    {
        _bookService = bookService;
        _chapterService = chapterService;
        _commentService = commentService;
        _bookUpvoteService = bookUpvoteService;
        _bookRatingService = bookRatingService;
        _bookmarkService = bookmarkService;
    }

    // GET /api/books
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBooks([FromQuery] ApiGetAllBooksRequest request)
    {
        var pagerRequest = new PagerRequest(
            request.PageIndex,
            request.PageSize);

        var getAllBooksResponse = await _bookService.GetAllBooksAsync(
            new GetAllBooksRequest(
                request.Keyword,
                request.GenreIds,
                request.IsDone,
                pagerRequest));

        var apiGetAllBooksResponse = new ApiGetAllBooksResponse(
            getAllBooksResponse.BookVms,
            getAllBooksResponse.PagerResponse.PageIndex,
            getAllBooksResponse.PagerResponse.PageSize,
            getAllBooksResponse.PagerResponse.TotalPages);

        return Ok(new ApiSuccessResult<ApiGetAllBooksResponse>(apiGetAllBooksResponse));
    }

    // POST /api/books
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateBook([FromForm] ApiCreateBookRequest request)
    {
        var thumbnailImageExt = Path.GetExtension(request.ThumbnailImage?.FileName);
        var thumbnailImageStream = request.ThumbnailImage?.OpenReadStream();

        try
        {
            var createBookRequest = new CreateBookRequest(
                request.Name,
                request.ShortDescription,
                request.LongDescription,
                request.IsRecommended,
                request.IsDone,
                request.Slug,
                request.AuthorId,
                thumbnailImageStream,
                thumbnailImageExt);

            var createBookResponse = await _bookService.CreateBookAsync(createBookRequest);

            return Ok(new ApiSuccessResult<BookVm>(createBookResponse.BookVm));
        }
        catch (AuthorNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (BookAlreadyExistsException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (InvalidImageExtensionException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    // GET /api/books/{id}
    [HttpGet("{bookId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBookById([FromRoute] int bookId)
    {
        var getBookByIdResponse = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(bookId));
        if (getBookByIdResponse.BookVm is null)
        {
            return Ok(new ApiErrorResult("The book with given Id does not exist."));
        }

        return Ok(new ApiSuccessResult<BookVm>(getBookByIdResponse.BookVm));
    }

    // GET /api/books/recommended
    [HttpGet("recommended")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRecommendedBooks()
    {
        var getRecommendedBooksResponse = await _bookService.GetRecommendedBooksAsync();
        var response = new ApiGetRecommendedBooksResponse(
            getRecommendedBooksResponse.BookVms);

        return Ok(new ApiSuccessResult<ApiGetRecommendedBooksResponse>(response));
    }

    // GET /api/books/chapters/latest
    [HttpGet("chapters/latest")]
    [AllowAnonymous]
    public async Task<IActionResult> GetLatestCreatedChapters([FromQuery] int numberOfChapters)
    {
        var getLatestCreatedChaptersResponse = await _chapterService.GetLatestCreatedChaptersAsync(
            new GetLatestCreatedChaptersRequest(numberOfChapters));
        var response = new ApiGetLatestCreatedChaptersResponse(
            getLatestCreatedChaptersResponse.ChapterVms);

        return Ok(new ApiSuccessResult<ApiGetLatestCreatedChaptersResponse>(response));
    }

    // POST /api/books/{bookId}/chapters
    [HttpPost("{bookId:int}/chapters")]
    public async Task<IActionResult> CreateBookChapter([FromRoute] int bookId,
        [FromBody] ApiCreateBookChapterRequest request)
    {
        try
        {
            var createBookChapterResponse = await _chapterService.CreateBookChapterAsync(
                new CreateBookChapterRequest(
                    bookId,
                    request.Name,
                    request.WordCount,
                    request.Content));

            return Ok(new ApiSuccessResult<ChapterVm>(createBookChapterResponse.ChapterVm));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (InvalidImageExtensionException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    // GET /api/books/{id}/chapters
    [HttpGet("{bookId:int}/chapters")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBookChapters([FromRoute] int bookId,
        [FromQuery] ApiGetAllBookChaptersRequest request)
    {
        var pagerRequest = new PagerRequest(
            request.PageIndex,
            request.PageSize);

        var getAllBookChaptersResponse = await _chapterService.GetAllBookChaptersAsync(
            new GetAllBookChaptersRequest(
                bookId,
                pagerRequest));

        var response = new ApiGetAllBookChaptersResponse(
            getAllBookChaptersResponse.ChapterVms,
            request.PageIndex,
            request.PageSize,
            getAllBookChaptersResponse.PagerResponse.TotalPages,
            getAllBookChaptersResponse.PagerResponse.TotalRecords);

        return Ok(new ApiSuccessResult<ApiGetAllBookChaptersResponse>(response));
    }

    // GET /api/books/{bookId}/chapters/{index}
    [HttpGet("{bookId:int}/chapters/{index:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetChapterByIndex([FromRoute] int bookId, [FromRoute] int index)
    {
        try
        {
            var getBookChapterByIndexResponse = await _chapterService.GetBookChapterByIndexAsync(
                new GetBookChapterByIndexRequest(bookId, index));

            if (getBookChapterByIndexResponse.ChapterVm is null)
            {
                return Ok(new ApiErrorResult("The chapter with given Index does not exist."));
            }

            return Ok(new ApiSuccessResult<ChapterVm>(getBookChapterByIndexResponse.ChapterVm));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    // GET /api/books/{slug}
    [HttpGet("{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBookBySlug([FromRoute] string slug)
    {
        var getBookBySlugResponse = await _bookService.GetBookBySlugAsync(new GetBookBySlugRequest(slug));
        if (getBookBySlugResponse.BookVm is null)
            return Ok(new ApiErrorResult("The book with given Slug does not exist."));

        var countBookUpvoteResponse = await _bookUpvoteService.CountBookUpvoteByBookIdAsync(
            new CountBookUpvoteByBookIdRequest(getBookBySlugResponse.BookVm.Id));

        var getAllBookmarksByBookIdResponse = await _bookmarkService.GetAllBookmarksByBookIdAsync(
            new GetAllBookmarksByBookIdRequest(getBookBySlugResponse.BookVm.Id));

        var response = new ApiGetBookBySlugResponse(
            getBookBySlugResponse.BookVm,
            countBookUpvoteResponse.TotalUpvotes,
            getAllBookmarksByBookIdResponse.BookmarkVms.Count);

        return Ok(new ApiSuccessResult<ApiGetBookBySlugResponse>(response));
    }

    [HttpPost("{bookId:int}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateBook([FromRoute] int bookId, [FromForm] ApiUpdateBookRequest request)
    {
        Stream? thumbnailImage = null;
        string? fileExtension = null;

        if (request.ThumbnailImage is not null)
        {
            thumbnailImage = request.ThumbnailImage.OpenReadStream();
            fileExtension = Path.GetExtension(request.ThumbnailImage.FileName);
        }

        try
        {
            var updateBookResponse = await _bookService.UpdateBookAsync(
                new UpdateBookRequest(
                    bookId,
                    request.Name,
                    request.ShortDescription,
                    request.LongDescription,
                    request.IsRecommended,
                    request.Slug,
                    request.AuthorId,
                    request.IsDone,
                    thumbnailImage,
                    fileExtension,
                    request.GenreCheckboxItems));

            return Ok(new ApiSuccessResult<BookVm>(updateBookResponse.BookVm));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (BookAlreadyExistsException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (InvalidImageExtensionException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpDelete("{bookId:int}")]
    public async Task<IActionResult> DeleteBook([FromRoute] int bookId)
    {
        try
        {
            await _bookService.DeleteBookAsync(
                new DeleteBookRequest(
                    bookId));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpPost("{bookId:int}/comments")]
    [Authorize]
    public async Task<IActionResult> CreateBookComment([FromRoute] int bookId,
        [FromBody] ApiCreateBookCommentRequest request)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            var createBookCommentResponse = await _commentService.CreateBookCommentAsync(
                new CreateBookCommentRequest(
                    bookId,
                    userId,
                    request.Content,
                    request.ParentId));

            return Ok(new ApiSuccessResult<CommentVm>(createBookCommentResponse.CommentVm));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpDelete("{bookId:int}/comments/{commentId:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteBookComment([FromRoute] int bookId, [FromRoute] int commentId)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            // Set forceDelete = false, so the user can only delete their own comments
            await _commentService.DeleteBookCommentAsync(
                new DeleteBookCommentRequest(
                    commentId,
                    bookId,
                    userId,
                    false));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (CommentNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet("{bookId:int}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBookComments([FromRoute] int bookId,
        [FromQuery] ApiGetAllBookCommentsRequest request)
    {
        try
        {
            var getAllBookCommentsResponse = await _commentService.GetAllBookCommentsAsync(
                new GetAllBookCommentsRequest(
                    bookId,
                    request.ParentId,
                    request.Asc,
                    new PagerRequest(request.PageIndex, request.PageSize)));

            var response = new ApiGetAllBookCommentsResponse(
                getAllBookCommentsResponse.CommentVms,
                request.PageIndex,
                request.PageSize,
                getAllBookCommentsResponse.PagerResponse.TotalPages,
                getAllBookCommentsResponse.PagerResponse.TotalRecords);

            return Ok(new ApiSuccessResult<ApiGetAllBookCommentsResponse>(response));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpPost("{bookId:int}/upvotes")]
    [Authorize]
    public async Task<IActionResult> CreateBookUpvote([FromRoute] int bookId)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            var createBookUpvoteResponse = await _bookUpvoteService.CreateBookUpvoteAsync(
                new CreateBookUpvoteRequest(userId, bookId));

            return Ok(new ApiSuccessResult<BookUpvoteVm>(createBookUpvoteResponse.BookUpvoteVm));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpDelete("{bookId:int}/upvotes")]
    [Authorize]
    public async Task<IActionResult> DeleteBookUpvote([FromRoute] int bookId)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            await _bookUpvoteService.DeleteBookUpvoteAsync(
                new DeleteBookUpvoteRequest(userId, bookId));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet("{bookId:int}/upvotes")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBookUpvotes([FromRoute] int bookId, [FromQuery] Guid? userId)
    {
        try
        {
            var getAllBookUpvotesResponse = await _bookUpvoteService.GetAllBookUpvotesAsync(
                new GetAllBookUpvotesRequest(
                    bookId, userId));

            var response = new ApiGetAllBookUpvotesResponse(getAllBookUpvotesResponse.BookUpvoteVms);

            return Ok(new ApiSuccessResult<ApiGetAllBookUpvotesResponse>(response));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet("most_reading")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMostReadingBooks([FromQuery] int numberOfBooks)
    {
        var getMostReadingBooksResponse = await _bookService.GetMostReadingBooks(
            new GetMostReadingBooksRequest(numberOfBooks));

        var response = new ApiGetMostReadingBooksResponse(getMostReadingBooksResponse.BookVms);

        return Ok(new ApiSuccessResult<ApiGetMostReadingBooksResponse>(response));
    }

    [HttpPost("{bookId:int}/ratings")]
    [Authorize]
    public async Task<IActionResult> CreateBookRating([FromRoute] int bookId, [FromBody] ApiCreateBookRatingRequest request)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            var createRatingResponse = await _bookRatingService.CreateBookRatingAsync(
                new CreateBookRatingRequest(
                    userId,
                    bookId,
                    request.Content,
                    request.Star));

            return Ok(new ApiSuccessResult<BookRatingVm>(createRatingResponse.BookRatingVm));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpDelete("{bookId:int}/ratings")]
    [Authorize]
    public async Task<IActionResult> DeleteBookRating([FromRoute] int bookId)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            await _bookRatingService.DeleteBookRatingAsync(
                new DeleteBookRatingRequest(
                    userId,
                    bookId));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (BookRatingNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpPut("{bookId:int}/ratings")]
    [Authorize]
    public async Task<IActionResult> UpdateBookRating([FromRoute] int bookId, [FromBody] ApiUpdateBookRatingRequest request)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            var updateBookRatingResponse =  await _bookRatingService.UpdateBookRatingAsync(
                new UpdateBookRatingRequest(
                    userId,
                    bookId,
                    request.Content,
                    request.Star));

            return Ok(new ApiSuccessResult<BookRatingVm>(updateBookRatingResponse.BookRatingVm));
        }
        catch (BookRatingNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
    
    [HttpGet("{bookId:int}/ratings")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBookRatings([FromRoute] int bookId, [FromQuery] ApiGetAllBookRatingsRequest request)
    {
        try
        {
            var getAllRatingsResponse =  await _bookRatingService.GetAllBookRatingsByBookIdAsync(
                new GetAllBookRatingsByBookIdRequest(
                    bookId,
                    request.UserId,
                    new PagerRequest(request.PageIndex, request.PageSize)));

            var pagerResponse = getAllRatingsResponse.PagerResponse;

            var response = new ApiGetAllBookRatingsResponse(
                getAllRatingsResponse.BookRatingVms,
                pagerResponse.PageIndex,
                pagerResponse.PageSize,
                pagerResponse.TotalPages,
                pagerResponse.TotalRecords);
            
            return Ok(new ApiSuccessResult<ApiGetAllBookRatingsResponse>(response));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet("{bookId:int}/ratings/overview")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBookRatingsOverview([FromRoute] int bookId)
    {
        try
        {
            var getOverviewResponse =  await _bookRatingService.GetOverviewAsync(
                new GetOverviewRequest(
                    bookId));

            var response = new ApiGetBookRatingsOverviewResponse(getOverviewResponse.RatingsByStar); 
            
            return Ok(new ApiSuccessResult<ApiGetBookRatingsOverviewResponse>(response));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
    
    private Guid GetUserIdFromClaim()
    {
        string userId = User.Claims.First(
            x => x.Type == JwtRegisteredClaimNames.Sid).Value;
        return new Guid(userId);
    }
}