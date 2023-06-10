using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Catalog.Authors;
using miranaSolution.Dtos.Catalog.Authors;
using miranaSolution.Dtos.Common;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.BackendApi.Controllers;

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
    public async Task<IActionResult> GetAll()
    {
        var authorList = await _authorService.GetAll();
        return Ok(new ApiSuccessResult<List<AuthorDto>>(authorList));
    }
}