using Microsoft.AspNetCore.Http;

namespace miranaSolution.DTOs.Auth.Users;

public class UserUpdateInfoRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public IFormFile? Avatar { get; set; }
}