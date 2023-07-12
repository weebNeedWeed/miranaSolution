using Microsoft.AspNetCore.Identity;

namespace miranaSolution.Data.Entities;

public class AppRole : IdentityRole<Guid>
{
    public string Description { get; set; } = String.Empty;
}