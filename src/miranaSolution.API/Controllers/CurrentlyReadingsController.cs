using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.CurrentlyReadings;
using miranaSolution.DTOs.Core.CurrentlyReading;
using miranaSolution.Services.Core.CurrentlyReadings;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CurrentlyReadingsController : ControllerBase
{
    private readonly ICurrentlyReadingService _currentlyReadingService;

    public CurrentlyReadingsController(ICurrentlyReadingService currentlyReadingService)
    {
        _currentlyReadingService = currentlyReadingService;
    }

    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] ApiAddBookRequest request)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            var addBookResponse = await _currentlyReadingService.AddBookAsync(
                new AddBookRequest(
                    request.BookId,
                    userId,
                    request.ChapterIndex));

            return Ok(new ApiSuccessResult<CurrentlyReadingVm>(addBookResponse.CurrentlyReadingVm));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (ChapterNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentlyReadingBooks([FromQuery] int? bookId)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            var getResponse = await _currentlyReadingService.GetCurrentlyReadingBooksAsync(
                new GetCurrentlyReadingBooksRequest(
                    userId, bookId));
            var response = new ApiGetCurrentlyReadingBooksResponse(getResponse.CurrentlyReadingVms);
            
            return Ok(new ApiSuccessResult<ApiGetCurrentlyReadingBooksResponse>(response));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpDelete("book/{bookId:int}")]
    public async Task<IActionResult> RemoveBook([FromRoute] int bookId)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            await _currentlyReadingService.RemoveBookAsync(
                new RemoveBookRequest(
                    userId, bookId));
            
            return Ok(new ApiSuccessResult<object>());
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
    
    private Guid GetUserIdFromClaim()
    {
        var userId = User.Claims.First(
            x => x.Type == JwtRegisteredClaimNames.Sid).Value;
        return new Guid(userId);
    }
}