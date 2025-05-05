using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Entity.Model;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Utilities;

public class Jwt
{
    private readonly IConfiguration _configuration;
    public Jwt(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerarJwt(User user, int rolId)
    {
        var userClaims = new[]  
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, rolId.ToString()),
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var JwtConfig = new JwtSecurityToken(
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(JwtConfig);
    }
}
