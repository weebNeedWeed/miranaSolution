using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.API.ViewModels.Authors;
using miranaSolution.DTOs.Core.Authors;
using Newtonsoft.Json;

namespace miranaSolution.Admin.Controllers;

[Authorize]
[AutoValidateAntiforgeryToken]
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
    public async Task<IActionResult> Create([FromForm] ApiCreateAuthorRequest request)
    {
        var response = await _authorsApiService.CreateAuthorAsync(request);
        if (response.Status == "fail")
        {
            var errors = (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string,string>>(response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }

        if (response.Status == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }
        
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
    {
        await _authorsApiService.DeleteAuthorAsync(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit([FromRoute] int id)
    {
        ViewData["AuthorId"] = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] int id,[FromForm] ApiUpdateAuthorRequest request)
    {
        var response = await _authorsApiService.UpdateAuthorAsync(id, request);
        ViewData["AuthorId"] = id;
        if (response.Status == "fail")
        {
            var errors = (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string,string>>(response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }

        if (response.Status == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }
        
        return RedirectToAction("Index");
    }
}