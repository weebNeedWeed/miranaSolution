using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Users;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.API.Controllers;

[ApiController]
[Route("auth")]
[Authorize]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("information")]
    public async Task<IActionResult> GetUserInformation()
    {
        var userName = GetUserNameFromClaims();
        
        var getUserByUserNameResponse = await _userService.GetUserByUserNameAsync(
            new GetUserByUserNameRequest(userName));

        if (getUserByUserNameResponse.UserVm is null)
        {
            return Ok(new ApiErrorResult("User was not found."));
        }
    
        return Ok(new ApiSuccessResult<UserVm>(getUserByUserNameResponse.UserVm));
    }
    
    // TODO: Implement the credentials validation here
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] ApiRegisterUserRequest request)
    {
        try
        {
            var registerUserResponse = await _userService.RegisterUserAsync(
                new RegisterUserRequest(
                    request.FirstName,
                    request.LastName,
                    request.UserName,
                    request.Email,
                    request.Password));
            
            return Ok(new ApiSuccessResult<UserVm>(registerUserResponse.UserVm));
        }
        catch (UserAlreadyExistsException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
    
    [HttpPost("authenticate")]
    [AllowAnonymous]
    public async Task<IActionResult> AuthenticateUser([FromBody] ApiAuthenticateUserRequest request)
    {
        try
        {
            var authenticateUserResponse = await _userService.AuthenticateUserAsync(
                new AuthenticateUserRequest(request.UserName, request.Password));

            var userVm = authenticateUserResponse.UserVm;
            var response = new ApiAuthenticateUserResponse(
                userVm.Id,
                userVm.FirstName,
                userVm.LastName,
                userVm.UserName,
                userVm.Email,
                userVm.Avatar,
                authenticateUserResponse.Token);
                
            return Ok(new ApiSuccessResult<ApiAuthenticateUserResponse>(response));
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
    
    [HttpPost("information")]
    [Consumes("multipart/form-data")]
    [Authorize]
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
    }
    
    // TODO: Implement validation
    [HttpPost("password")]
    [Authorize]
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
    }

    private string GetUserNameFromClaims()
    {
        var userName = User
            .Claims
            .First(x => x.Type == ClaimTypes.NameIdentifier).Value;

        return userName;
    }
}