using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using StageFlow.Application.Auth;

namespace StageFlow.Api;

public class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
    public string CreateToken(Guid userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = options.Value.Issuer,
            Audience = options.Value.Audience,
            Expires = DateTime.UtcNow.AddMinutes(options.Value.ExpiresInMinutes),
            SigningCredentials = credentials,
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = userId.ToString(),
                [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
            }
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }
}