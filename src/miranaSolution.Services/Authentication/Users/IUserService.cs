using miranaSolution.DTOs.Auth.Users;
using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.Services.Authentication.Users;

public interface IUserService
{
    // Task<string> Authentication(UserAuthenticationRequest request);
    //
    // Task<UserDto> Register(UserRegisterRequest request);
    //
    // Task<UserDto> GetByEmail(string email);
    //
    // Task<UserDto> GetByUserName(string userName);
    //
    // Task<UserDto> UpdateInfo(Guid id, UserUpdateInfoRequest request);
    // Task<UserDto> UpdatePassword(Guid id, UserUpdatePasswordRequest request);
    
    // New methods
    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);

    Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request);
}