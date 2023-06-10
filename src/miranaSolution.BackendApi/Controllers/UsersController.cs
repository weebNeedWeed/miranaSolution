using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Auth.Users;
using miranaSolution.Dtos.Auth.Users;
using miranaSolution.Dtos.Common;
using miranaSolution.Utilities.Exceptions;

namespace miranaSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (await _userService.GetByEmail(request.Email) is not null)
            {
                return Ok(new ApiFailResult(new Dictionary<string, List<string>>
                {
                    {nameof(request.Email), new List<string>{"Duplicated email."} }
                }));
            }

            if (await _userService.GetByUserName(request.UserName) is not null)
            {
                return Ok(new ApiFailResult(new Dictionary<string, List<string>>
                {
                    {nameof(request.UserName), new List<string>{"Duplicated User Name."} }
                }));
            }

            UserDto newUser;

            try
            {
                newUser = await _userService.Register(request);
            }
            catch (MiranaBusinessException ex)
            {
                return Ok(new ApiFailResult(new Dictionary<string, List<string>>
                {
                    {nameof(request.UserName), new List<string>{ex.Message} }
                }));
            }

            return Ok(new ApiSuccessResult<UserDto>(newUser));
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationRequest request)
        {
            string accessToken;

            try
            {
                accessToken = await _userService.Authentication(request);
            }
            catch (MiranaBusinessException ex)
            {
                return Ok(new ApiFailResult(new Dictionary<string, List<string>>
                {
                    {nameof(request.UserName), new List<string>{ex.Message} }
                }));
            }

            return Ok(new ApiSuccessResult<object>(new { AccessToken = accessToken }));
        }
    }
}