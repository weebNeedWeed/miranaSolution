﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Users;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.Bookmarks;
using miranaSolution.DTOs.Core.BookRatings;
using miranaSolution.DTOs.Core.BookUpvotes;
using miranaSolution.DTOs.Core.CommentReactions;
using miranaSolution.DTOs.Core.Comments;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Core.Bookmarks;
using miranaSolution.Services.Core.BookRatings;
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
    private readonly IBookmarkService _bookmarkService;
    private readonly IBookRatingService _bookRatingService;
    private readonly IBookUpvoteService _bookUpvoteService;
    private readonly ICommentReactionService _commentReactionService;
    private readonly ICommentService _commentService;
    private readonly IUserService _userService;

    public UsersController(IUserService userService, ICommentService commentService,
        ICommentReactionService commentReactionService, IBookmarkService bookmarkService,
        IBookUpvoteService bookUpvoteService, IBookRatingService bookRatingService)
    {
        _userService = userService;
        _commentService = commentService;
        _commentReactionService = commentReactionService;
        _bookmarkService = bookmarkService;
        _bookUpvoteService = bookUpvoteService;
        _bookRatingService = bookRatingService;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile()
    {
        var userName = GetUserNameFromClaims();

        var getUserByUserNameResponse = await _userService.GetUserByUserNameAsync(
            new GetUserByUserNameRequest(userName));

        if (getUserByUserNameResponse.UserVm is null) return Ok(new ApiErrorResult("The user does not exist."));

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
        var totalBookmarks = getAllBookmarkByUserIdResponse.BookmarkVms.Count;

        // Calculate the summation of upvotes that user has done
        var countBookUpvoteByUserIdResponse = await _bookUpvoteService.CountBookUpvoteByUserIdAsync(
            new CountBookUpvoteByUserIdRequest(userVm.Id));
        var totalUpvotes = countBookUpvoteByUserIdResponse.TotalUpvotes;

        var getAllBookRatingsByUserIdResponse = await _bookRatingService.GetAllBookRatingsByUserIdAsync(
            new GetAllBookRatingsByUserIdRequest(userVm.Id));
        var totalRatings = getAllBookRatingsByUserIdResponse.BookRatingVms.Count;

        var response = new ApiGetUserProfileResponse(
            userVm,
            totalComments,
            totalReactions,
            totalBookmarks,
            totalUpvotes,
            totalRatings);

        return Ok(new ApiSuccessResult<ApiGetUserProfileResponse>(response));
    }

    [HttpGet("{userId}/profile")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserProfileById([FromRoute] string userId)
    {
        var getUserByIdResponse = await _userService.GetUserByIdAsync(new GetUserByIdRequest(
            new Guid(userId)));
        if (getUserByIdResponse.UserVm is null) return Ok(new ApiErrorResult("The user with given Id does not exist."));

        var editedUserVm = getUserByIdResponse.UserVm with
        {
            Email = ""
        };

        return Ok(new ApiSuccessResult<UserVm>(editedUserVm));
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

    [HttpPut("read_chapter_count/increase")]
    [Authorize]
    public async Task<IActionResult> IncreaseReadChapterCount()
    {
        var userName = GetUserNameFromClaims();

        var getUserByUserNameResponse = await _userService.GetUserByUserNameAsync(
            new GetUserByUserNameRequest(userName));

        if (getUserByUserNameResponse.UserVm is null) return Ok(new ApiErrorResult("The user does not exist."));

        try
        {
            var increaseResponse = await _userService.IncreaseReadChapterCountBy1Async(
                new IncreaseReadChapterCountBy1Request(getUserByUserNameResponse.UserVm.Id));
            var response = new ApiIncreaseReadChapterCountResponse(increaseResponse.ReadChapterCount);
            return Ok(new ApiSuccessResult<ApiIncreaseReadChapterCountResponse>(response));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpPut("read_book_count/increase")]
    [Authorize]
    public async Task<IActionResult> IncreaseReadBookCount()
    {
        var userName = GetUserNameFromClaims();

        var getUserByUserNameResponse = await _userService.GetUserByUserNameAsync(
            new GetUserByUserNameRequest(userName));

        if (getUserByUserNameResponse.UserVm is null) return Ok(new ApiErrorResult("The user does not exist."));

        try
        {
            var increaseResponse = await _userService.IncreaseReadBookCountBy1Async(
                new IncreaseReadBookCountBy1Request(getUserByUserNameResponse.UserVm.Id));
            var response = new ApiIncreaseReadBookCountResponse(increaseResponse.ReadBookCount);
            return Ok(new ApiSuccessResult<ApiIncreaseReadBookCountResponse>(response));
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet]
    [Authorize(Roles = RolesConstant.Administrator)]
    public async Task<IActionResult> GetAllUsers([FromQuery] ApiGetAllUsersRequest request)
    {
        var getAllUsersResponse = await _userService.GetAllUsersAsync(
            new GetAllUsersRequest(
                request.Keyword,
                new PagerRequest(request.PageIndex, request.PageSize)));

        var (userVms, pagerResponse) = getAllUsersResponse;

        var response = new ApiGetAllUsersResponse(
            userVms,
            pagerResponse.PageIndex,
            pagerResponse.PageSize,
            pagerResponse.TotalRecords,
            pagerResponse.TotalPages);

        return Ok(new ApiSuccessResult<ApiGetAllUsersResponse>(response));
    }

    [HttpPost("{userId}/roles")]
    [Authorize(Roles = RolesConstant.Administrator)]
    public async Task<IActionResult> AssignRoles([FromRoute] string userId, [FromForm] ApiAssignRolesRequest request)
    {
        try
        {
            await _userService.AssignRolesAsync(
                new AssignRolesRequest(
                    new Guid(userId),
                    request.RoleCheckboxItems));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet("{userId}/roles")]
    [Authorize(Roles = RolesConstant.Administrator)]
    public async Task<IActionResult> GetRolesByUserId([FromRoute] string userId)
    {
        try
        {
            var getRolesByUserIdResponse = await _userService.GetRolesByUserIdAsync(
                new GetRolesByUserIdRequest(
                    new Guid(userId)));
            var result = new ApiGetRolesByUserIdResponse(getRolesByUserIdResponse.RoleVms);
            
            return Ok(new ApiSuccessResult<ApiGetRolesByUserIdResponse>(result));
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