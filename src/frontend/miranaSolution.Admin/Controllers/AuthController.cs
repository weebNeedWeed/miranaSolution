using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using miranaSolution.Admin.Models.Auth;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.Utilities.Constants;
using Newtonsoft.Json;

namespace miranaSolution.Admin.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly IAuthApiService _authApiService;
    private readonly IConfiguration _configuration;
    private readonly JwtOptions _jwtOptions;

    public AuthController(IConfiguration configuration, IAuthApiService authApiService, IOptions<JwtOptions> jwtOptions)
    {
        _configuration = configuration;
        _authApiService = authApiService;
        _jwtOptions = jwtOptions.Value;
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

        var response = await _authApiService.AuthenticateUserAsync(new AuthenticateUserRequest(
            loginViewModel.UserName,
            loginViewModel.Password));

        if (response.Status == "fail" || response.Status == "error")
        {
            ModelState.AddModelError("", "Unknown error.");
            return View(loginViewModel);
        }
        
        var claimsIdentity = GetClaimsIdentity(response.Data.Token);
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

        HttpContext.Session.SetString(Constants.AccessToken, response.Data.Token);

        if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
    
    private ClaimsIdentity? GetClaimsIdentity(string accessToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                ValidateIssuer = true,
                ValidateAudience = true,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            if (!jwtToken.Claims.First(x => x.Type.Equals(ClaimTypes.Role)).Value.Contains(RolesConstant.Administrator)) return null;

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