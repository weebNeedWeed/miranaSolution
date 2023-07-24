using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.Extensions;
using miranaSolution.API.ViewModels.Authors;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Core.Authors;
using miranaSolution.Services.Core.Authors;
using miranaSolution.Services.Exceptions;
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
        var response = new ApiGetAllAuthorsResponse(getAllAuthorsResponse.AuthorVms);
        
        return Ok(new ApiSuccessResult<ApiGetAllAuthorsResponse>(response));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthor([FromBody] ApiCreateAuthorRequest request)
    {
        var createAuthorResponse = await _authorService.CreateAuthorAsync(
                new CreateAuthorRequest(request.Name));

        return Ok(new ApiSuccessResult<AuthorVm>(createAuthorResponse.AuthorVm));
    }
    
    [HttpPatch("{authorId:int}")]
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
}