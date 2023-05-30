using Microsoft.AspNetCore.Mvc;
using miranaSolution.Admin.Models.Books;

namespace miranaSolution.Admin.Controllers;

public class BooksController : Controller
{
    // GET /books
    [HttpGet]
    public IActionResult Index()
    {
        return View();
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