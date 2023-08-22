using miranaSolution.Data.Entities;

namespace miranaSolution.Services.Systems.JwtTokenGenerators;

public interface IJwtTokenGenerator
{
    Task<string> GenerateTokenAsync(AppUser user);
}