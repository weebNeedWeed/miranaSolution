using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Admin.Services.Interfaces;

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
}