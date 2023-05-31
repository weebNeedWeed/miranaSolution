using Microsoft.AspNetCore.Mvc;
using miranaSolution.Admin.Models.Books;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.Dtos.Catalog.Books;

namespace miranaSolution.Admin.Controllers;

public class BooksController : Controller
{
    private readonly IBooksApiService _booksApiService;

    public BooksController(IBooksApiService booksApiService)
    {
        _booksApiService = booksApiService;
    }
    
    // GET /books
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int pageIndex = 1, [FromQuery] int pageSize=10, [FromQuery] string keyword = "")
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
    public IActionResult Create()
    {
        return View();
    }
    
    // POST /books/create
    [HttpPost]
    public IActionResult Create(BookCreateViewModel bookCreateViewModel)
    {
        return View();
    }
}