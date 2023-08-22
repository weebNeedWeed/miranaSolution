using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Roles;
using miranaSolution.Services.Authentication.Roles;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.API.Controllers;

[Authorize(Roles = RolesConstant.Administrator)]
[ApiController]
[Route("/api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var getRolesResponse = await _roleService.GetAllRolesAsync();
        var result = new ApiGetAllRolesResponse(
            getRolesResponse.RoleVms);

        return Ok(new ApiSuccessResult<ApiGetAllRolesResponse>(result));
    }
}