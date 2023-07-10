using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using miranaSolution.Admin.Models.Auth;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.DTOs.Auth.Users;
using miranaSolution.Utilities.Constants;
using Newtonsoft.Json;

namespace miranaSolution.Admin.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly IUsersApiService _usersApiService;
    private readonly IConfiguration _configuration;

    public AuthController(IUsersApiService usersApiService, IConfiguration configuration)
    {
        _configuration = configuration;
        _usersApiService = usersApiService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login([FromQuery] string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl ?? "/";
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginViewModel loginViewModel, [FromQuery] string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl ?? "/";

        if (!ModelState.IsValid) return View(loginViewModel);

        var response = await _usersApiService.Authenticate(new UserAuthenticationRequest()
        {
            Password = loginViewModel.Password,
            RememberMe = false,
            UserName = loginViewModel.UserName
        });

        if (response.Status == "fail" || response.Status == "error")
        {
            ModelState.AddModelError("", "Unknown error.");
            return View(loginViewModel);
        }

        var accessToken = JsonConvert.DeserializeObject<dynamic>(response.Data.ToString()).accessToken;

        var claimsIdentity = GetClaimsIdentity((string)accessToken);
        if (claimsIdentity == null)
        {
            ModelState.AddModelError("", "Unknown error.");
            return View(loginViewModel);
        }

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = false
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        HttpContext.Session.SetString("accessToken", (string)accessToken);

        if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    private ClaimsIdentity? GetClaimsIdentity(string accessToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Issuer"],
                ValidateIssuer = true,
                ValidateAudience = true,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            if (!jwtToken.Claims.First(x => x.Type == "roles").Value.Contains(RolesConstant.Administrator)) return null;

            var claimsIdentity = new ClaimsIdentity(
                jwtToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return claimsIdentity;
        }
        catch
        {
            return null;
        }
    }
}