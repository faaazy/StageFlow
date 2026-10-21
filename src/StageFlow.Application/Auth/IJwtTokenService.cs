namespace StageFlow.Application.Auth;

public interface IJwtTokenService
{
    string CreateToken(Guid userId);
}