using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StageFlow.Api;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal user, out Guid userId)
    {
        var sub = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        return Guid.TryParse(sub, out userId);
    }
}