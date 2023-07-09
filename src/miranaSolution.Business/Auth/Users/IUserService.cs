using miranaSolution.Dtos.Auth.Users;

namespace miranaSolution.Business.Auth.Users
{
    public interface IUserService
    {
        Task<string> Authentication(UserAuthenticationRequest request);

        Task<UserDto> Register(UserRegisterRequest request);

        Task<UserDto> GetByEmail(string email);

        Task<UserDto> GetByUserName(string userName);

        Task<UserDto> UpdateInfo(Guid id,UserUpdateInfoRequest request);
        Task<UserDto> UpdatePassword(Guid id,UserUpdatePasswordRequest request);
    }
}