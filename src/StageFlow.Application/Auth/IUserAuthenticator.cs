namespace StageFlow.Application.Auth;

public interface IUserAuthenticator
{
    Task<UserAuthResult> AuthenticateAsync(
        string email,
        string password
    );
}

public record UserAuthResult(
    bool Succeeded,
    Guid? UserId,
    IReadOnlyList<string> Errors
);