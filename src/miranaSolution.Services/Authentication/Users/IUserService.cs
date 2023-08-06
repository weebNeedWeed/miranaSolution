using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.Services.Authentication.Users;

public interface IUserService
{
    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);

    Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request);

    Task<GetUserByUserNameResponse> GetUserByUserNameAsync(GetUserByUserNameRequest request);

    Task<GetUserByEmailResponse> GetUserByEmailAsync(GetUserByEmailRequest request);

    Task<UpdateUserProfileResponse> UpdateUserProfileAsync(UpdateUserProfileRequest request);

    Task<UpdateUserPasswordResponse> UpdateUserPasswordAsync(UpdateUserPasswordRequest request);

    Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdRequest request);

    Task DeleteUserAsync(DeleteUserRequest request);

    Task<IncreaseReadBookCountBy1Response> IncreaseReadBookCountBy1Async(IncreaseReadBookCountBy1Request request);

    Task<IncreaseReadChapterCountBy1Response> IncreaseReadChapterCountBy1Async(IncreaseReadChapterCountBy1Request request);
}