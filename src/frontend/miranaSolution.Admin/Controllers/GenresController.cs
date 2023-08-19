using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.API.ViewModels.Genres;
using Newtonsoft.Json;

namespace miranaSolution.Admin.Controllers;

[AutoValidateAntiforgeryToken]
[Authorize]
public class GenresController : Controller
{
    private readonly IGenresApiService _genresApiService;

    public GenresController(IGenresApiService genresApiService)
    {
        _genresApiService = genresApiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var getGenresResult = await _genresApiService.GetAllGenresAsync();

        return View(getGenresResult.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ApiCreateGenreRequest request)
    {
        var response = await _genresApiService.CreateGenreAsync(request);
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

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int id)
    {
        ViewData["GenreId"] = id;
        var getGenreResult = await _genresApiService.GetGenreByIdAsync(id);
        var genre = getGenreResult.Data;

        var viewModel = new ApiUpdateGenreRequest(
            genre.Name);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ApiUpdateGenreRequest request)
    {
        ViewData["GenreId"] = id;

        if (!ModelState.IsValid)
        {
            return View(request);
        }
        
        var response = await _genresApiService.UpdateGenreAsync(id, request);
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

    [HttpPost]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _genresApiService.DeleteGenreAsync(id);
        return RedirectToAction("Index");
    }
}