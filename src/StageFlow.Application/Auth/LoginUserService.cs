namespace StageFlow.Application.Auth;

public class LoginUserService(IUserAuthenticator authenticator)
{
    public async Task<UserAuthResult> LoginAsync(
        string email,
        string password
    )
    {
        return await authenticator.AuthenticateAsync(email, password);
    }
}