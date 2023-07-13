using miranaSolution.Data.Entities;

namespace miranaSolution.Services.Systems.JwtTokenGenerators;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser user);
}