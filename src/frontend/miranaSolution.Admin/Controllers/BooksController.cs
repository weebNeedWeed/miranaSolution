using System.Text;
using Refit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.DTOs.Catalog.Books.Chapters;
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
        var books = await _booksApiService.GetPaging(new BookGetPagingRequest()
        {
            PageSize = pageSize,
            PageIndex = pageIndex,
            Keyword = keyword
        });

        return View(books.Data);
    }

    // GET /books/create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var authors = (await _authorsApiService.GetAll()).Data;
        ViewBag.Authors = new SelectList(authors, nameof(AuthorDto.Id), nameof(AuthorDto.Name));

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
        var book = await _booksApiService.GetById(id);
        if (book.Status is null || book.Status == "fail" || book.Status == "error") return RedirectToAction("Index");

        ViewBag.Book = book.Data;

        var chapters = await _booksApiService.GetChaptersPaging(id, new ChapterGetPagingRequest()
        {
            PageSize = pageSize,
            PageIndex = pageIndex
            // Keyword = keyword
        });

        return View(chapters.Data);
    }

    // POST /books/create
    [HttpPost]
    [Consumes("multipart/form-data")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create([FromForm] BookCreateRequest request)
    {
        var authors = (await _authorsApiService.GetAll()).Data;
        ViewBag.Authors = new SelectList(authors, nameof(AuthorDto.Id), nameof(AuthorDto.Name));

        if (!ModelState.IsValid) return View(request);

        var memoryStream = new MemoryStream();
        request.ThumbnailImage.OpenReadStream().CopyTo(memoryStream);
        var byteArrayPart = new ByteArrayPart(memoryStream.ToArray(), request.ThumbnailImage.FileName);

        var token = HttpContext.Session.GetString("accessToken")!;

        var response = await _booksApiService.Create(request.Name,
            request.ShortDescription,
            request.LongDescription,
            slug: request.Slug,
            authorId: request.AuthorId,
            isRecommended: request.IsRecommended,
            thumbnailImage: byteArrayPart,
            authorization: "Bearer " + token);

        if (response.Status == "error")
        {
            ModelState.AddModelError("", "Unknown error.");
            return View(request);
        }

        if (response.Status == "fail")
        {
            var errorDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(response.Data.ToString());
            foreach (var pair in errorDict)
            {
                var errorString = new StringBuilder();
                foreach (var error in pair.Value) errorString.Append(error);

                ModelState.AddModelError(pair.Key, errorString.ToString());
            }

            return View(request);
        }

        return RedirectToAction("Index");
    }

    // GET /books/{id}/chapters/create
    [HttpGet("[controller]/{id:int}/chapters/create")]
    public async Task<IActionResult> AddChapter([FromRoute] int id)
    {
        var book = await _booksApiService.GetById(id);
        if (book.Status is null || book.Status == "fail" || book.Status == "error") return RedirectToAction("Index");

        ViewBag.Book = book.Data;

        return View();
    }

    // POST /books/{id}/chapters/create
    [HttpPost("[controller]/{id:int}/chapters/create")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> AddChapter([FromRoute] int id, [FromForm] ChapterCreateRequest request)
    {
        var book = await _booksApiService.GetById(id);
        if (book.Status is null || book.Status == "fail" || book.Status == "error") return RedirectToAction("Index");

        ViewBag.Book = book.Data;

        var result = await _booksApiService.AddChapter(id, request);
        if (result.Status == "fail" || result.Status == "error")
        {
            ModelState.AddModelError("", !string.IsNullOrEmpty(result.Message) ? result.Message : "Unknown error");
            return View(request);
        }

        return RedirectToAction("ShowChapters", new { id = book.Data.Id });
    }
}