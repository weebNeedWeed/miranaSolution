using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.Data;

public class SampleDataSeeder
{
    private readonly MiranaDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SeedingOptions _seedingOptions;

    public SampleDataSeeder(MiranaDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IOptions<SeedingOptions> seedingOptions)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _seedingOptions = seedingOptions.Value;
    }

    public async Task SeedAllAsync()
    {
        if (!await _roleManager.RoleExistsAsync(RolesConstant.User))
        {
            var userRole = new AppRole
            {
                Name = RolesConstant.User,
                Description = RolesConstant.User
            };
            await _roleManager.CreateAsync(userRole);
        }

        if (!await _roleManager.RoleExistsAsync(RolesConstant.Administrator))
        {
            var adminRole = new AppRole
            {
                Name = RolesConstant.Administrator,
                Description = RolesConstant.Administrator
            };
            await _roleManager.CreateAsync(adminRole);
        }

        if (!await _context.Users.AnyAsync())
        {
            var admin = new AppUser
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = _seedingOptions.DefaultAdminUsername,
                Email = "admin@admin.com",
            };
            await _userManager.CreateAsync(admin, _seedingOptions.DefaultAdminPassword);
            await _userManager.AddToRoleAsync(admin, RolesConstant.Administrator);
        }
    }
}