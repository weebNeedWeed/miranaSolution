using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Genres;
using miranaSolution.DTOs.Core.Genres;
using miranaSolution.Services.Core.Genres;
using miranaSolution.Services.Exceptions;
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
        var response = new ApiGetAllGenreResponse(getAllGenresResponse.GenreVms);

        return Ok(new ApiSuccessResult<ApiGetAllGenreResponse>(response));
    }

    [HttpGet("{genreId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetGenreById([FromRoute] int genreId)
    {
        var getGenreByIdResponse = await _genreService.GetGenreByIdAsync(
            new GetGenreByIdRequest(genreId));

        if (getGenreByIdResponse.GenreVm is null)
            return Ok(new ApiErrorResult("The genre with given Id does not exist."));

        return Ok(new ApiSuccessResult<GenreVm>(getGenreByIdResponse.GenreVm));
    }

    [HttpPost]
    public async Task<IActionResult> CreateGenre([FromBody] ApiCreateGenreRequest request)
    {
        var createGenreResponse = await _genreService.CreateGenreAsync(
            new CreateGenreRequest(
                request.Name));
        return Ok(new ApiSuccessResult<GenreVm>(createGenreResponse.GenreVm));
    }

    [HttpPatch("{genreId:int}")]
    public async Task<IActionResult> UpdateGenre([FromRoute] int genreId, [FromBody] ApiUpdateGenreRequest request)
    {
        try
        {
            var createGenreResponse = await _genreService.UpdateGenreAsync(
                new UpdateGenreRequest(
                    genreId,
                    request.Name));

            return Ok(new ApiSuccessResult<GenreVm>(createGenreResponse.GenreVm));
        }
        catch (GenreNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpDelete("{genreId:int}")]
    public async Task<IActionResult> DeleteGenre([FromRoute] int genreId)
    {
        try
        {
            await _genreService.DeleteGenreAsync(
                new DeleteGenreRequest(genreId));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (GenreNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
}