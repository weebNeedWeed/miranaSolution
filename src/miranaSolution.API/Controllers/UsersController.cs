using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Users;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.DTOs.Core.Comments;
using miranaSolution.Services.Authentication.Users;
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

    public UsersController(IUserService userService, ICommentService commentService)
    {
        _userService = userService;
        _commentService = commentService;
    }
    
    [HttpGet("information")]
    public async Task<IActionResult> GetUserInformation()
    {
        var userName = GetUserNameFromClaims();
        
        var getUserByUserNameResponse = await _userService.GetUserByUserNameAsync(
            new GetUserByUserNameRequest(userName));

        if (getUserByUserNameResponse.UserVm is null)
        {
            return Ok(new ApiErrorResult("The user does not exist."));
        }

        var countCommentByUserIdResponse = await _commentService.CountCommentByUserIdAsync(
            new CountCommentByUserIdRequest(getUserByUserNameResponse.UserVm.Id));
        var userVm = getUserByUserNameResponse.UserVm;
        
        var response = new ApiGetUserInformationResponse(
            userVm.Id,
            userVm.FirstName,
            userVm.LastName,
            userVm.UserName,
            userVm.Email,
            userVm.Avatar,
            countCommentByUserIdResponse.TotalComments);
        
        return Ok(new ApiSuccessResult<ApiGetUserInformationResponse>(response));
    }
    
    [HttpPost("information")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateUserInformation([FromForm] ApiUpdateUserInformationRequest request)
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

            var updateUserInformationResponse = await _userService.UpdateUserInformationAsync(
                new UpdateUserInformationRequest(
                    userName,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    avatarStream,
                    avatarExtension
                ));

            var userVm = updateUserInformationResponse.UserVm;
            var response = new ApiUpdateUserInformationResponse(
                userVm.Id,
                userVm.FirstName,
                userVm.LastName,
                userVm.UserName,
                userVm.Email,
                userVm.Avatar);

            return Ok(new ApiSuccessResult<ApiUpdateUserInformationResponse>(response));
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
    
    // TODO: Implement validation
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
                    request.NewPassword));

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