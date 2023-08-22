using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Admin.Extensions;
using miranaSolution.Admin.Services.Interfaces;
using miranaSolution.API.ViewModels.Authentication;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Users;
using miranaSolution.DTOs.Common;
using Newtonsoft.Json;

namespace miranaSolution.Admin.Controllers;

[Authorize]
[AutoValidateAntiforgeryToken]
public class UsersController : Controller
{
    private readonly IUsersApiService _usersApiService;
    private readonly IAuthApiService _authApiService;
    private readonly IRolesApiService _rolesApiService;
    private readonly IConfiguration _configuration;

    public UsersController(IUsersApiService usersApiService, IAuthApiService authApiService, IRolesApiService rolesApiService, IConfiguration configuration)
    {
        _usersApiService = usersApiService;
        _authApiService = authApiService;
        _rolesApiService = rolesApiService;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ApiGetAllUsersRequest request)
    {
        var getUsersResponse = await _usersApiService
            .GetAllUsersAsync(request);

        return View(getUsersResponse.Data);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ApiRegisterUserRequest request)
    {
        if (!ModelState.IsValid) return View(request);
        
        var response = await _authApiService.RegisterUserAsync(request);

        if (response.Status == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }

        if (response.Status == "fail")
        {
            var errors =
                (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _usersApiService.DeleteUserAsync(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> AssignRoles([FromRoute] string id)
    {
        ViewData["UserId"] = id;
        
        var getAllAvailableRolesResponse = await _rolesApiService.GetAllRolesAsync();
        var getUserRolesResponse = await _usersApiService.GetRolesByUserId(id);

        var rand = new Random();

        var roleCheckBoxItems = getAllAvailableRolesResponse.Data.Roles
            .Select(x => new CheckboxItem
            {
                Id = rand.Next(1,10),
                Label = x.Name,
                IsChecked = getUserRolesResponse.Data.Roles.Any(_ => _.Name.Equals(x.Name))
            }).ToList();

        return View(new ApiAssignRolesRequest(roleCheckBoxItems));
    }

    [HttpPost]
    public async Task<IActionResult> AssignRoles([FromRoute] string id, [FromForm] ApiAssignRolesRequest request)
    {
        ViewData["UserId"] = id;

        var token = HttpContext.Session.GetString(Constants.AccessToken);
        using var client = new HttpClient();
        
        client.BaseAddress = new Uri(_configuration["BaseAddress"]);
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", token!);
        
        var _response = await client.PostAsync(
            $"/api/users/{id}/roles",request.ToFormData());
        
        var content = await _response.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<ApiResult<dynamic>>(content);
        
        if (response.Status == "error")
        {
            ViewData[Constants.Error] = response.Message;
            return View(request);
        }

        if (response.Status == "fail")
        {
            var errors =
                (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(
                    response.Data.ToString());
            ViewData[Constants.Error] = errors.Values.ElementAt(0);
            return View(request);
        }
        
        return RedirectToAction("Index");
    }
}