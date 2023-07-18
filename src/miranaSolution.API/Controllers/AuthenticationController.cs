using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Authentication;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Users;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.API.Controllers;

[ApiController]
[Route("auth")]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }
    
    // TODO: Implement the credentials validation here
    [HttpPost("register")]
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
}