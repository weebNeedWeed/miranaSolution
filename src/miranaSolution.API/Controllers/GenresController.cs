using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.Genres;
using miranaSolution.Services.Core.Genres;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RolesConstant.Administrator)]
public class GenresController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllGenres()
    {
        var getAllGenresResponse = await _genreService.GetAllGenresAsync();
        return Ok(new ApiSuccessResult<List<GenreVm>>(getAllGenresResponse.GenreVms));
    }
}