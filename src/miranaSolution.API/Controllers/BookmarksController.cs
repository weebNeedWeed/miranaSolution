using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using miranaSolution.API.ViewModels.Bookmarks;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Core.Bookmarks;
using miranaSolution.DTOs.Core.Chapters.Exceptions;
using miranaSolution.Services.Core.Bookmarks;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookmarksController : ControllerBase
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarksController(IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBookmark([FromBody] ApiCreateBookmarkRequest request)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            var createBookmarkResponse = await _bookmarkService.CreateBookmarkAsync(
                new CreateBookmarkRequest(
                    userId,
                    request.BookId,
                    request.ChapterIndex));

            return Ok(new ApiSuccessResult<BookmarkVm>(createBookmarkResponse.BookmarkVm));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (ChapterNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBookmarks([FromQuery] int? bookId)
    {
        var userId = GetUserIdFromClaim();
        try
        {
            var getAllBookmarksByUserIdResponse = await _bookmarkService.GetAllBookmarksByUserIdAsync(
                new GetAllBookmarksByUserIdRequest(userId, bookId));
            var response = new ApiGetAllBookmarksResponse(getAllBookmarksByUserIdResponse.BookmarkVms);

            return Ok(new ApiSuccessResult<ApiGetAllBookmarksResponse>(response));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpDelete("book/{bookId:int}")]
    public async Task<IActionResult> DeleteBookmark([FromRoute] int bookId)
    {
        var userId = GetUserIdFromClaim();

        try
        {
            await _bookmarkService.DeleteBookmarkAsync(
                new DeleteBookmarkRequest(
                    userId,
                    bookId));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (BookNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (BookmarkNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    private Guid GetUserIdFromClaim()
    {
        var userId = User.Claims
            .First(x => x.Type == JwtRegisteredClaimNames.Sid).Value;
        return new Guid(userId);
    }
}