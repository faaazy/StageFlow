namespace StageFlow.Application.Auth;

public interface IUserAccountCreator
{
    Task<UserRegistrationResult> CreateAsync(
        string email,
        string password
    );
}

public record UserRegistrationResult(
    bool Succeeded,
    Guid? UserId,
    IReadOnlyList<string> Errors
);