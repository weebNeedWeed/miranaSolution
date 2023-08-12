using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.API.ViewModels.Authors;
using Newtonsoft.Json;

namespace miranaSolution.Admin.Controllers;

[Authorize]
public class AuthorsController : Controller
{
    private readonly IAuthorsApiService _authorsApiService;

    public AuthorsController(IAuthorsApiService authorsApiService)
    {
        _authorsApiService = authorsApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _authorsApiService.GetAllAuthorAsync();
        return View(result.Data);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create([FromForm] ApiCreateAuthorRequest request)
    {
        var response = await _authorsApiService.CreateAuthor(request);
        if (response.Message == "fail")
        {
            var errors = (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string,string>>(response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }

        if (response.Message == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }
        
        return RedirectToAction("Index");
    }
}