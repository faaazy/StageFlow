using StageFlow.Application.Auth;

namespace StageFlow.Application.Schedule;

public class RegisterUserService(
    IUserAccountCreator accountCreator)
{
    public async Task<UserRegistrationResult> RegisterAsync(
            string email,
            string password
        )
    {
        return await accountCreator.CreateAsync(email, password);
    }
}