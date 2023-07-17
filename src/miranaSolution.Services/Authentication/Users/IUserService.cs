using miranaSolution.DTOs.Auth.Users;
using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.Services.Authentication.Users;

public interface IUserService
{
    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);

    Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request);

    Task<GetUserByUserNameResponse> GetUserByUserNameAsync(GetUserByUserNameRequest request);
    
    Task<GetUserByEmailResponse> GetUserByEmailAsync(GetUserByEmailRequest request);

    Task<UpdateUserInformationResponse> UpdateUserInformationAsync(UpdateUserInformationRequest request);
    
    Task<UpdateUserPasswordResponse> UpdateUserPasswordAsync(UpdateUserPasswordRequest request);

    Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdRequest request);

    Task DeleteUserAsync(DeleteUserRequest request);
}