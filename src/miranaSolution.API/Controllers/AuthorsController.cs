using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Authors;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Core.Authors;
using miranaSolution.Services.Core.Authors;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Systems.Mails;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService, IMailService mailService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllAuthors()
    {
        var getAllAuthorsResponse = await _authorService.GetAllAuthorsAsync();
        var response = new ApiGetAllAuthorsResponse(getAllAuthorsResponse.AuthorVms);

        return Ok(new ApiSuccessResult<ApiGetAllAuthorsResponse>(response));
    }

    [HttpPost]
    [Authorize(Roles = RolesConstant.Administrator)]
    public async Task<IActionResult> CreateAuthor([FromBody] ApiCreateAuthorRequest request)
    {
        var createAuthorResponse = await _authorService.CreateAuthorAsync(
            new CreateAuthorRequest(request.Name));

        return Ok(new ApiSuccessResult<AuthorVm>(createAuthorResponse.AuthorVm));
    }

    [HttpPut("{authorId:int}")]
    [Authorize(Roles = RolesConstant.Administrator)]
    public async Task<IActionResult> UpdateAuthor([FromRoute] int authorId, [FromBody] ApiUpdateAuthorRequest request)
    {
        try
        {
            var updateAuthorResponse = await _authorService.UpdateAuthorAsync(
                new UpdateAuthorRequest(
                    authorId,
                    request.Name));
            return Ok(new ApiSuccessResult<AuthorVm>(updateAuthorResponse.AuthorVm));
        }
        catch (AuthorNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpDelete("{authorId:int}")]
    [Authorize(Roles = RolesConstant.Administrator)]
    public async Task<IActionResult> DeleteAuthor([FromRoute] int authorId)
    {
        try
        {
            await _authorService.DeleteAuthorAsync(
                new DeleteAuthorRequest(
                    authorId));
            return Ok(new ApiSuccessResult<object>());
        }
        catch (AuthorNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet("{authorId:int}/books")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBooksByAuthorId([FromRoute] int authorId, [FromQuery] int numberOfBooks = 10)
    {
        try
        {
            var getAllBookByAuthorIdResponse = await _authorService
                .GetAllBookByAuthorIdAsync(new GetAllBooksByAuthorIdRequest(authorId, numberOfBooks));

            var response = new ApiGetAllBooksByAuthorIdResponse(
                getAllBookByAuthorIdResponse.BookVms);

            return Ok(new ApiSuccessResult<ApiGetAllBooksByAuthorIdResponse>(response));
        }
        catch (AuthorNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
}