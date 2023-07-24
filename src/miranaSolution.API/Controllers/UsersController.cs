using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Users;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.DTOs.Core.Bookmarks;
using miranaSolution.DTOs.Core.BookUpvotes;
using miranaSolution.DTOs.Core.CommentReactions;
using miranaSolution.DTOs.Core.Comments;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Core.Bookmarks;
using miranaSolution.Services.Core.BookUpvotes;
using miranaSolution.Services.Core.CommentReactions;
using miranaSolution.Services.Core.Comments;
using miranaSolution.Services.Exceptions;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICommentService _commentService;
    private readonly ICommentReactionService _commentReactionService;
    private readonly IBookmarkService _bookmarkService;
    private readonly IBookUpvoteService _bookUpvoteService;

    public UsersController(IUserService userService, ICommentService commentService, ICommentReactionService commentReactionService, IBookmarkService bookmarkService, IBookUpvoteService bookUpvoteService)
    {
        _userService = userService;
        _commentService = commentService;
        _commentReactionService = commentReactionService;
        _bookmarkService = bookmarkService;
        _bookUpvoteService = bookUpvoteService;
    }
    
    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        var userName = GetUserNameFromClaims();
        
        var getUserByUserNameResponse = await _userService.GetUserByUserNameAsync(
            new GetUserByUserNameRequest(userName));

        if (getUserByUserNameResponse.UserVm is null)
        {
            return Ok(new ApiErrorResult("The user does not exist."));
        }

        var userVm = getUserByUserNameResponse.UserVm;
        
        // Calculate the summation of comments that user has written
        var countCommentByUserIdResponse = await _commentService.CountCommentByUserIdAsync(
            new CountCommentByUserIdRequest(userVm.Id));
        var totalComments = countCommentByUserIdResponse.TotalComments;
        
        // Calculate the summation of reactions that user has reacted
        var countReactionByUserIdResponse = await _commentReactionService.CountCommentReactionByUserIdAsync(
            new CountCommentReactionByUserIdRequest(userVm.Id));
        var totalReactions = countReactionByUserIdResponse.TotalReactions;
        
        // Calculate the summation of bookmarks that user has marked
        var getAllBookmarkByUserIdResponse = await _bookmarkService.GetAllBookmarksByUserIdAsync(
            new GetAllBookmarksByUserIdRequest(userVm.Id, null));
        var totalBookmarks = getAllBookmarkByUserIdResponse.BookmarkVms.Count();
        
        // Calculate the summation of upvotes that user has done
        var countBookUpvoteByUserIdResponse = await _bookUpvoteService.CountBookUpvoteByUserIdAsync(
            new CountBookUpvoteByUserIdRequest(userVm.Id));
        var totalUpvotes = countBookUpvoteByUserIdResponse.TotalUpvotes;
        
        var response = new ApiGetUserProfileResponse(
            userVm,
            totalComments,
            totalReactions,
            totalBookmarks,
            totalUpvotes);
        
        return Ok(new ApiSuccessResult<ApiGetUserProfileResponse>(response));
    }
    
    [HttpPost("profile")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateUserProfile([FromForm] ApiUpdateUserProfileRequest request)
    {
        var userName = GetUserNameFromClaims();

        try
        {
            Stream? avatarStream = null;
            string? avatarExtension = null;

            if (request.Avatar is not null)
            {
                avatarStream = request.Avatar.OpenReadStream();
                avatarExtension = Path.GetExtension(request.Avatar.FileName);
            }

            var updateUserInformationResponse = await _userService.UpdateUserProfileAsync(
                new UpdateUserProfileRequest(
                    userName,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    avatarStream,
                    avatarExtension
                ));

            var userVm = updateUserInformationResponse.UserVm;
            var response = new ApiUpdateUserProfileResponse(
                userVm.Id,
                userVm.FirstName,
                userVm.LastName,
                userVm.UserName,
                userVm.Email,
                userVm.Avatar);

            return Ok(new ApiSuccessResult<ApiUpdateUserProfileResponse>(response));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (InvalidImageExtensionException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
    
    [HttpPost("password")]
    public async Task<IActionResult> UpdateUserPassword([FromBody] ApiUpdateUserPasswordRequest request)
    {
        var userName = GetUserNameFromClaims();

        try
        {
            var updateUserPasswordResponse = await _userService.UpdateUserPasswordAsync(
                new UpdateUserPasswordRequest(
                    userName,
                    request.CurrentPassword,
                    request.NewPassword,
                    request.NewPasswordConfirmation));

            var userVm = updateUserPasswordResponse.UserVm;
            var response = new ApiUpdateUserPasswordResponse(
                userVm.Id,
                userVm.FirstName,
                userVm.LastName,
                userVm.UserName,
                userVm.Email,
                userVm.Avatar);

            return Ok(new ApiSuccessResult<ApiUpdateUserPasswordResponse>(response));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (InvalidCredentialException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpDelete("{userId}")]
    [Authorize(Roles = RolesConstant.Administrator)]
    public async Task<IActionResult> DeleteUser([FromRoute] string userId)
    {
        try
        {
            await _userService.DeleteUserAsync(
                new DeleteUserRequest(new Guid(userId)));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    private string GetUserNameFromClaims()
    {
        var userName = User
            .Claims
            .First(x => x.Type == ClaimTypes.NameIdentifier).Value;

        return userName;
    }
}