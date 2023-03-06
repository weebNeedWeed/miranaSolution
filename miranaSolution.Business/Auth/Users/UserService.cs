using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using miranaSolution.Data.Entities;
using miranaSolution.Dtos.Auth.Users;
using miranaSolution.Utilities.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace miranaSolution.Business.Auth.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> Authentication(UserAuthenticationRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, false);

            if (!result.Succeeded)
            {
                throw new MiranaBusinessException("Cannot authenticate user");
            }

            var user = await _userManager.FindByNameAsync(request.UserName);

            return GenerateToken(user);
        }

        public async Task<UserDto> Register(UserRegisterRequest request)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserRegisterRequest, AppUser>();
                cfg.CreateMap<AppUser, UserDto>();
            });
            var mapper = config.CreateMapper();

            var newUser = mapper.Map<AppUser>(request);

            var createResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!createResult.Succeeded)
            {
                throw new MiranaBusinessException("Cannot create new User");
            }

            var returnData = mapper.Map<UserDto>(newUser);

            return returnData;
        }

        private string GenerateToken(AppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, user.Email),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Exp, TimeSpan.FromMinutes(60).ToString())
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}