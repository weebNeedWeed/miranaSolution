using miranaSolution.Dtos.Auth.Users;

namespace miranaSolution.Business.Auth.Users
{
    public interface IUserService
    {
        Task<string> Authentication(UserAuthenticationRequest request);

        Task<UserDto> Register(UserRegisterRequest request);
    }
}