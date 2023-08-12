using Refit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.API.ViewModels.Books;
using miranaSolution.DTOs.Core.Authors;
using Newtonsoft.Json;

namespace miranaSolution.Admin.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize]
public class BooksController : Controller
{
    private readonly IBooksApiService _booksApiService;
    private readonly IAuthorsApiService _authorsApiService;

    public BooksController(IBooksApiService booksApiService, IAuthorsApiService authorsApiService)
    {
        _booksApiService = booksApiService;
        _authorsApiService = authorsApiService;
    }

    // GET /books
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10,
        [FromQuery] string keyword = "")
    {
        var books = await _booksApiService.GetAllBooksAsync(
            new ApiGetAllBooksRequest(
                keyword,
                null,
                null,
                null,
                pageIndex,
                pageSize));

        return View(books.Data);
    }

    // GET /books/create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var authors = (await _authorsApiService.GetAllAuthorAsync()).Data;
        ViewBag.Authors = new SelectList(authors.Authors, nameof(AuthorVm.Id), nameof(AuthorVm.Name));

        return View();
    }

    // GET /books/{id}/chapters
    [HttpGet("[controller]/{id:int}/chapters")]
    public async Task<IActionResult> ShowChapters(
        [FromRoute] int id,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string keyword = "")
    {
        var book = await _booksApiService.GetBookByIdAsync(id);
        if (book.Status is null || book.Status == "fail" || book.Status == "error") return RedirectToAction("Index");

        ViewBag.Book = book.Data;

        var chapters = await _booksApiService.GetAllBookChaptersAsync(id, new ApiGetAllBookChaptersRequest()
        {
            PageSize = pageSize,
            PageIndex = pageIndex
        });

        return View(chapters.Data);
    }

    // POST /books/create
    [HttpPost]
    [Consumes("multipart/form-data")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create([FromForm] ApiCreateBookRequest request)
    {
        var authors = (await _authorsApiService.GetAllAuthorAsync()).Data;
        ViewBag.Authors = new SelectList(authors.Authors, nameof(AuthorVm.Id), nameof(AuthorVm.Name));

        if (!ModelState.IsValid) return View(request);

        var memoryStream = new MemoryStream();
        await request.ThumbnailImage.OpenReadStream().CopyToAsync(memoryStream);
        var byteArrayPart = new ByteArrayPart(memoryStream.ToArray(), request.ThumbnailImage.FileName);

        var token = HttpContext.Session.GetString(Constants.AccessToken)!;

        var response = await _booksApiService.CreateAsync(request.Name,
            request.ShortDescription,
            request.LongDescription,
            slug: request.Slug,
            authorId: request.AuthorId,
            isDone: request.IsDone,
            isRecommended: request.IsRecommended,
            thumbnailImage: byteArrayPart);

        if (response.Status == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }

        if (response.Status == "fail")
        {
            var errors = (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string,string>>(response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }

        return RedirectToAction("Index");
    }

    // GET /books/{id}/chapters/create
    [HttpGet("[controller]/{id:int}/chapters/create")]
    public async Task<IActionResult> AddChapter([FromRoute] int id)
    {
        var book = await _booksApiService.GetBookByIdAsync(id);
        if (book.Status is null || book.Status == "fail" || book.Status == "error") return RedirectToAction("Index");

        ViewBag.Book = book.Data;

        return View();
    }

    // POST /books/{id}/chapters/create
    [HttpPost("[controller]/{id:int}/chapters/create")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> AddChapter([FromRoute] int id, [FromForm] ApiCreateBookChapterRequest request)
    {
        var book = await _booksApiService.GetBookByIdAsync(id);
        if (book.Status is null || book.Status == "fail" || book.Status == "error") return RedirectToAction("Index");

        ViewBag.Book = book.Data;

        var accessToken = HttpContext.Session.GetString(Constants.AccessToken);

        var response = await _booksApiService.CreateBookChapterAsync(
            id, 
            request);
        if (response.Status == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }

        if (response.Status == "fail")
        {
            var errors = (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string,string>>(response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }

        return RedirectToAction("ShowChapters", new { id = book.Data.Id });
    }
}