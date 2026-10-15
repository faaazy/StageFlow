using Microsoft.AspNetCore.Identity;
using StageFlow.Application.Auth;

namespace StageFlow.Infrastructure.Identity;

public class IdentityUserAuthenticator(UserManager<ApplicationUser> userManager)
    : IUserAuthenticator
{
    public async Task<UserAuthResult> AuthenticateAsync(string email, string password)
    {
        var applicationUser = await userManager.FindByEmailAsync(email);

        if(applicationUser is null)
        {
            return new UserAuthResult(false, null, ["Invalid email or password."]);
        }

        var checkPassword = await userManager.CheckPasswordAsync(applicationUser, password);

        if(!checkPassword)
        {
            return new UserAuthResult(false, null, ["Invalid email or password."]);
        }

        return new UserAuthResult(true, applicationUser.Id, []);
    }
}