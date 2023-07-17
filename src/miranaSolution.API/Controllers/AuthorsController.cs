using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.Authors;
using miranaSolution.Services.Core.Authors;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RolesConstant.Administrator)]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllAuthors()
    {
        var getAllAuthorsResponse = await _authorService.GetAllAuthorsAsync();
        return Ok(new ApiSuccessResult<List<AuthorVm>>(getAllAuthorsResponse.AuthorVms));
    }
}