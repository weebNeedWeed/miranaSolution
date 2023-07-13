using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using miranaSolution.Data.Entities;
using miranaSolution.DTOs.Auth.Users;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Systems.Files;
using miranaSolution.Services.Systems.JwtTokenGenerators;

namespace miranaSolution.Services.Authentication.Users;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IFileService _fileService;
    private readonly JwtOptions _jwtOptions;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        IConfiguration configuration, IFileService fileService, IOptions<JwtOptions> jwtOptions, JwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _fileService = fileService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _jwtOptions = jwtOptions.Value;
    }

    // public async Task<string> Authentication(UserAuthenticationRequest request)
    // {
    //     var result =
    //         await _signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, false);
    //
    //     if (!result.Succeeded) throw new Exception("Invalid credentials.");
    //
    //     var user = await _userManager.FindByNameAsync(request.UserName);
    //
    //     return await GenerateToken(user);
    // }
    //
    // public async Task<UserDto> GetByEmail(string email)
    // {
    //     var config = new MapperConfiguration(cfg => cfg.CreateMap<AppUser, UserDto>());
    //     var mapper = config.CreateMapper();
    //
    //     var user = await _userManager.FindByEmailAsync(email);
    //     var returnData = mapper.Map<UserDto>(user);
    //
    //     return returnData;
    // }
    //
    // public async Task<UserDto> GetByUserName(string userName)
    // {
    //     var config = new MapperConfiguration(cfg => cfg.CreateMap<AppUser, UserDto>());
    //     var mapper = config.CreateMapper();
    //
    //     var user = await _userManager.FindByNameAsync(userName);
    //     var returnData = mapper.Map<UserDto>(user);
    //
    //     return returnData;
    // }
    //
    // public async Task<UserDto> UpdateInfo(Guid id, UserUpdateInfoRequest request)
    // {
    //     var user = await _userManager.FindByIdAsync(id.ToString());
    //     if (user is null) return null;
    //
    //     user.FirstName = request.FirstName;
    //     user.LastName = request.LastName;
    //     user.Email = request.Email;
    //
    //     if (request.Avatar is not null)
    //         user.Avatar = await SaveAvatar(request.Avatar);
    //
    //     await _userManager.UpdateAsync(user);
    //
    //     var config = new MapperConfiguration(cfg => cfg.CreateMap<AppUser, UserDto>());
    //     var mapper = config.CreateMapper();
    //
    //     return mapper.Map<UserDto>(user);
    // }
    //
    // public async Task<UserDto> UpdatePassword(Guid id, UserUpdatePasswordRequest request)
    // {
    //     var user = await _userManager.FindByIdAsync(id.ToString());
    //     if (user is null) throw new Exception("The user with given id does not exists.");
    //
    //     if (!await _userManager.CheckPasswordAsync(user, request.OldPassword)) throw new Exception("Invalid password.");
    //
    //     if (!request.NewPassword.Equals(request.NewPasswordConfirmation))
    //         throw new Exception("The new password confirmation must be equal to the new password.");
    //
    //     await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
    //
    //     var config = new MapperConfiguration(cfg => cfg.CreateMap<AppUser, UserDto>());
    //     var mapper = config.CreateMapper();
    //
    //     return mapper.Map<UserDto>(user);
    // }
    //
    // public async Task<UserDto> Register(UserRegisterRequest request)
    // {
    //     var config = new MapperConfiguration(cfg =>
    //     {
    //         cfg.CreateMap<UserRegisterRequest, AppUser>();
    //         cfg.CreateMap<AppUser, UserDto>();
    //     });
    //     var mapper = config.CreateMapper();
    //
    //     var newUser = mapper.Map<AppUser>(request);
    //
    //     var createResult = await _userManager.CreateAsync(newUser, request.Password);
    //
    //     if (!createResult.Succeeded) throw new Exception("Cannot create new User");
    //
    //     await _userManager.AddToRolesAsync(newUser, new[] { "User" });
    //
    //     var returnData = mapper.Map<UserDto>(newUser);
    //
    //     return returnData;
    // }
    //
    // private async Task<string> GenerateToken(AppUser user)
    // {
    //     var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    //     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    //
    //     var roles = await _userManager.GetRolesAsync(user);
    //     var rolesClaim = roles.Select(x => new Claim("roles", x));
    //
    //     var claims = new[]
    //     {
    //         new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
    //         new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
    //         new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
    //         new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
    //         new Claim(JwtRegisteredClaimNames.Email, user.Email),
    //         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //         new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Exp,
    //             TimeSpan.FromMinutes(60).ToString())
    //     };
    //
    //     var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
    //         _configuration["Jwt:Issuer"],
    //         claims.Concat(rolesClaim),
    //         expires: DateTime.Now.AddMinutes(60),
    //         signingCredentials: credentials);
    //
    //     return new JwtSecurityTokenHandler().WriteToken(token);
    // }
    //
    // private async Task<string> SaveAvatar(IFormFile avatar)
    // {
    //     var extension = Path.GetExtension(avatar.FileName);
    //     var newName = $"{Guid.NewGuid().ToString()}{extension}";
    //
    //     await _fileService.SaveFileAsync(
    //         avatar.OpenReadStream(), newName);
    //
    //     return _fileService.GetPath(newName);
    // }


    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is not null)
        {
            throw new UserAlreadyExistsException("The user with given Email already exists.");
        }
        
        user = await _userManager.FindByNameAsync(request.UserName);
        if (user is not null)
        {
            throw new UserAlreadyExistsException("The user with given UserName already exists.");
        }

        user = new AppUser()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.UserName
        };

        await _userManager.CreateAsync(user, request.Password);

        var userVm = new UserVm(
            user.Id,
            user.FirstName,
            user.LastName,
            user.UserName,
            user.Email,
            null);

        var response = new RegisterUserResponse(userVm);
        return response;
    }

    public async Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null)
        {
            throw new UserNotFoundException("The user with given Email does not exists.");
        }

        var signInResult = await _signInManager.PasswordSignInAsync(
            request.UserName,
            request.Password,
            false,
            false);

        if (!signInResult.Succeeded)
        {
            throw new InvalidCredentialException("Invalid credentials.");
        }

        var userVm = new UserVm(
            user.Id,
            user.FirstName,
            user.LastName,
            user.UserName,
            user.Email,
            user.Avatar);

        var token = _jwtTokenGenerator.GenerateToken(user);
        var response = new AuthenticateUserResponse(userVm, token);

        return response;
    }
}