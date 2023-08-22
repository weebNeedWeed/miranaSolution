using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.DTOs.Authentication.Roles;

namespace miranaSolution.Services.Authentication.Roles;

public class RoleService : IRoleService
{
    private readonly RoleManager<AppRole> _roleManager;

    public RoleService(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<GetAllRolesResponse> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var roleVms = roles.Select(x => new RoleVm(
            x.Id,
            x.Name,
            x.Description)).ToList();

        return new GetAllRolesResponse(roleVms);
    }
}