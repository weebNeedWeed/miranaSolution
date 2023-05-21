using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Catalog.Authors;
using miranaSolution.Dtos.Catalog.Authors;
using miranaSolution.Dtos.Common;

namespace miranaSolution.BackendApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var authorList = await _authorService.GetAll();
        return Ok(new ApiSuccessResult<List<AuthorDto>>(authorList));
    }
}