using System.Net.Http.Headers;
using Refit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using miranaSolution.Admin.Extensions;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.API.ViewModels.Books;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.Authors;
using Newtonsoft.Json;

namespace miranaSolution.Admin.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize]
[AutoValidateAntiforgeryToken]
public class BooksController : Controller
{
    private readonly IBooksApiService _booksApiService;
    private readonly IAuthorsApiService _authorsApiService;
    private readonly IGenresApiService _genresApiService;
    private readonly IConfiguration _configuration;

    public BooksController(IBooksApiService booksApiService, IAuthorsApiService authorsApiService,
        IGenresApiService genresApiService, IConfiguration configuration)
    {
        _booksApiService = booksApiService;
        _authorsApiService = authorsApiService;
        _genresApiService = genresApiService;
        _configuration = configuration;
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
    public async Task<IActionResult> Create([FromForm] ApiCreateBookRequest request)
    {
        var authors = (await _authorsApiService.GetAllAuthorAsync()).Data;
        ViewBag.Authors = new SelectList(authors.Authors, nameof(AuthorVm.Id), nameof(AuthorVm.Name));

        if (!ModelState.IsValid) return View(request);

        var memoryStream = new MemoryStream();
        await request.ThumbnailImage.OpenReadStream().CopyToAsync(memoryStream);
        var byteArrayPart = new ByteArrayPart(memoryStream.ToArray(), request.ThumbnailImage.FileName);

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
            var errors =
                (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    response.Data.ToString());
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
    public async Task<IActionResult> AddChapter([FromRoute] int id, [FromForm] ApiCreateBookChapterRequest request)
    {
        var book = await _booksApiService.GetBookByIdAsync(id);
        if (book.Status is null || book.Status == "fail" || book.Status == "error") return RedirectToAction("Index");

        ViewBag.Book = book.Data;

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
            var errors =
                (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }

        return RedirectToAction("ShowChapters", new { id = book.Data.Id });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteBook([FromForm] int bookId)
    {
        await _booksApiService.DeleteBookAsync(bookId);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int id)
    {
        ViewData["BookId"] = id;

        var result = await _booksApiService.GetBookByIdAsync(id);
        var book = result.Data;

        var result0 = await _authorsApiService.GetAllAuthorAsync();
        ViewBag.Authors = new SelectList(
            result0.Data.Authors,
            nameof(AuthorVm.Id),
            nameof(AuthorVm.Name),
            new { Id = book.AuthorId });

        var model = new ApiUpdateBookRequest(
            book.Name,
            book.ShortDescription,
            book.LongDescription,
            book.IsRecommended,
            book.Slug,
            book.AuthorId,
            book.IsDone,
            null
        );

        return View(model);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ApiUpdateBookRequest request)
    {
        ViewData["BookId"] = id;

        var result = await _booksApiService.GetBookByIdAsync(id);
        var book = result.Data;

        var result0 = await _authorsApiService.GetAllAuthorAsync();
        ViewBag.Authors = new SelectList(
            result0.Data.Authors,
            nameof(AuthorVm.Id),
            nameof(AuthorVm.Name),
            new { Id = book.AuthorId });
        
        if (!ModelState.IsValid) return View(request);

        StreamPart? streamPart = null;
        if (request.ThumbnailImage is not null)
        {
            streamPart = new StreamPart(
                request.ThumbnailImage.OpenReadStream(),
                request.ThumbnailImage.FileName);
        }

        var response = await _booksApiService.UpdateBookAsync(
            book.Id,
            request.Name,
            request.ShortDescription,
            request.LongDescription,
            request.IsRecommended,
            request.Slug,
            request.AuthorId,
            request.IsDone,
            streamPart);
        
        if (response.Status == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }

        if (response.Status == "fail")
        {
            var errors =
                (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }
        
        return RedirectToAction("Index");
    }

    [HttpGet("[controller]/{id:int}/genres")]
    public async Task<IActionResult> AssignGenres([FromRoute] int id)
    {
        ViewData["BookId"] = id;

        var result = await _booksApiService.GetBookByIdAsync(id);
        var book = result.Data;
        
        var checkboxItems = await GetGenreCheckboxItemsAsync(book.Id);
        var request = new ApiAssignGenresRequest(checkboxItems);
        
        return View(request);
    }
    
    [HttpPost("[controller]/{id:int}/genres")]
    public async Task<IActionResult> AssignGenres([FromRoute] int id,[FromForm] ApiAssignGenresRequest request)
    {
        ViewData["BookId"] = id;

        var result = await _booksApiService.GetBookByIdAsync(id);
        var book = result.Data;
        
        var token = HttpContext.Session.GetString(Constants.AccessToken);
        using var client = new HttpClient();
        
        client.BaseAddress = new Uri(_configuration["BaseAddress"]);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", token!);
        
        var _response = await client.PostAsync(
            $"/api/books/{book.Id}/genres",request.ToFormData());
        
        var content = await _response.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<ApiResult<dynamic>>(content);
        
        if (response.Status == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }

        if (response.Status == "fail")
        {
            var errors =
                (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }
        
        return RedirectToAction("Index");
    }
    
    private async Task<List<CheckboxItem>> GetGenreCheckboxItemsAsync(int bookId)
    {
        var result = await _booksApiService.GetAllGenresByBookIdAsync(bookId);
        var bookGenres = result.Data.Genres;

        var result2 = await _genresApiService.GetAllGenresAsync();
        var allGenres = result2.Data.Genres;

        var checkboxItems = allGenres.Select(x => new CheckboxItem
        {
            Id = x.Id,
            IsChecked = bookGenres.Any(_ => x.Id == _.Id),
            Label = x.Name
        }).ToList();

        return checkboxItems;
    }
}