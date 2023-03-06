using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Auth.Users;
using miranaSolution.Dtos.Auth.Users;
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
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserDto newUser;

            try
            {
                newUser = await _userService.Register(request);
            }
            catch (MiranaBusinessException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(newUser);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token;

            try
            {
                token = await _userService.Authentication(request);
            }
            catch (MiranaBusinessException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(token);
        }
    }
}