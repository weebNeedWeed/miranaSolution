using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Books;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.Services.Catalog.Books;
using miranaSolution.Services.Catalog.Chapters;
using miranaSolution.DTOs.Catalog.Books;
using miranaSolution.DTOs.Catalog.Chapters;
using miranaSolution.DTOs.Common;
using miranaSolution.Services.Exceptions;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RolesConstant.Administrator)]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IChapterService _chapterService;

    public BooksController(IBookService bookService, IChapterService chapterService)
    {
        _bookService = bookService;
        _chapterService = chapterService;
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
        var thumbnailImageExt = Path.GetExtension(request.ThumbnailImage.FileName);

        try
        {
            var userId = User.Claims.First(x => x.Type == "sid").Value;
            var createBookRequest = new CreateBookRequest(
                new Guid(userId),
                request.Name,
                request.ShortDescription,
                request.LongDescription,
                request.IsRecommended,
                request.IsDone,
                request.Slug,
                request.AuthorId,
                request.ThumbnailImage.OpenReadStream(),
                thumbnailImageExt);

            var createBookResponse = await _bookService.CreateBookAsync(createBookRequest);

            return Ok(new ApiSuccessResult<BookVm>(createBookResponse.BookVm));
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
            return Ok(new ApiErrorResult("Invalid book id."));
        }

        return Ok(new ApiSuccessResult<BookVm>(getBookByIdResponse.BookVm));
    }

    // GET /api/books/recommended
    [HttpGet("recommended")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRecommendedBooks()
    {
        var getRecommendedBooksResponse = await _bookService.GetRecommendedBooksAsync();
        return Ok(new ApiSuccessResult<List<BookVm>>(getRecommendedBooksResponse.BookVms));
    }

    // GET /api/books/chapters/latest
    [HttpGet("chapters/latest")]
    [AllowAnonymous]
    public async Task<IActionResult> GetLatestCreatedChapters([FromQuery] int numberOfChapters)
    {
        var getLatestCreatedChaptersResponse = await _chapterService.GetLatestCreatedChaptersAsync(
            new GetLatestCreatedChaptersRequest(numberOfChapters));
        
        return Ok(new ApiSuccessResult<List<ChapterVm>>(getLatestCreatedChaptersResponse.ChapterVms));
    }

    // POST /api/books/{bookId}/chapters
    [HttpPost("{bookId:int}/chapters")]
    public async Task<IActionResult> CreateBookChapter([FromRoute] int bookId, [FromBody] ApiCreateBookChapterRequest request)
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
    }

    // GET /api/books/{id}/chapters
    [HttpGet("{bookId:int}/chapters")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBookChapters([FromRoute] int bookId,[FromQuery] ApiGetAllBookChaptersRequest request)
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
            getAllBookChaptersResponse.PagerResponse.TotalPages);
        
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
    public async Task<IActionResult> GetBySlug([FromRoute] string slug)
    {
        var getBookBySlugResponse = await _bookService.GetBookBySlugAsync(new GetBookBySlugRequest(slug));
        if (getBookBySlugResponse.BookVm is null) 
            return Ok(new ApiErrorResult("The book with given Slug does not exist."));

        return Ok(new ApiSuccessResult<BookVm>(getBookBySlugResponse.BookVm));
    }
}