using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Auth.Users;
using miranaSolution.Dtos.Auth.Users;
using miranaSolution.Dtos.Common;
using miranaSolution.Utilities.Constants;
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

        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var userName = User
                .Claims
                .First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userService.GetByUserName(userName);

            if (user is null)
            {
                return Ok(new ApiErrorResult("User was not found."));
            }

            return Ok(new ApiSuccessResult<UserDto>(user));
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

        [HttpPost("info")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> UpdateInfo([FromForm] UserUpdateInfoRequest request)
        {
            var validExts = new string[] { ".jpg", ".png", ".jpeg" };
            if (request.Avatar is not null && !validExts.Contains(Path.GetExtension(request.Avatar.FileName)))
            {
                return Ok(new ApiFailResult( new Dictionary<string, List<string>>()
                {
                    {nameof(request.Avatar), new List<string>() {"Invalid image extension"}}
                }));
            }

            var userId = User.Claims.First(x => x.Type == "sid").Value;
            var result = await _userService.UpdateInfo(new Guid(userId), request);

            if (result is null)
            {
                return Ok(new ApiErrorResult());
            }

            return Ok(new ApiSuccessResult<UserDto>(result));
        }
    }
}