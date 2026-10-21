namespace StageFlow.Application.Auth;

public class LoginUserService(
        IUserAuthenticator authenticator,
        IJwtTokenService jwtTokenService
    )
{
    public async Task<LoginResult> LoginAsync(
        string email,
        string password
    )
    {
        var result = await authenticator.AuthenticateAsync(email, password);

        if (!result.Succeeded)
        {
            return new LoginResult(false, null, result.Errors);
        }

        var userId = result.UserId!.Value;

        var jwtToken = jwtTokenService.CreateToken(userId);

        return new LoginResult(true, jwtToken, []);
    }
}