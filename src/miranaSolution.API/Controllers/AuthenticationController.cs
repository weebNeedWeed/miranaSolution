using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Authentication;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Authentication.PasswordRecovery;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.Services.Authentication.PasswordRecovery;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.API.Controllers;

[ApiController]
[Route("auth")]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPasswordRecoveryService _passwordRecoveryService;

    public AuthenticationController(IUserService userService, IPasswordRecoveryService passwordRecoveryService)
    {
        _userService = userService;
        _passwordRecoveryService = passwordRecoveryService;
    }
    
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
                    request.Password,
                    request.PasswordConfirmation));

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

            var response = new ApiAuthenticateUserResponse(
                authenticateUserResponse.UserVm,
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

    [HttpPost("reset-password")]
    public async Task<IActionResult> PasswordReset([FromBody] ApiResetPasswordRequest request)
    {
        try
        {
            await _passwordRecoveryService.SendRecoveryEmailAsync(
                new SendRecoveryEmailRequest(request.Email, request.Callback));
            return Ok(new ApiSuccessResult<object>());
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
    
    [HttpPost("reset-password/redeem-token")]
    public async Task<IActionResult> RedeemToken([FromBody] ApiRedeemTokenRequest request)
    {
        try
        {
            await _passwordRecoveryService.RedeemTokenAsync(
                new RedeemTokenRequest(
                    request.Token,
                    request.Email,
                    request.NewPassword,
                    request.NewPasswordConfirmation));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (UserNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
        catch (InvalidTokenException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
}