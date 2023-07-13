using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using miranaSolution.Data.Entities;

namespace miranaSolution.Services.Systems.JwtTokenGenerators;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _jwtOptions;

    public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(AppUser user)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.UserName)
        };

        var signingCredentials = new SigningCredentials(
            key: new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
            algorithm: SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}