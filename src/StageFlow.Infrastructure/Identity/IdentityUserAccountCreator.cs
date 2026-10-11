using Microsoft.AspNetCore.Identity;
using StageFlow.Application.Auth;

namespace StageFlow.Infrastructure.Identity;

public class IdentityUserAccountCreator(UserManager<ApplicationUser> userManager) 
    : IUserAccountCreator
{   
    public async Task<UserRegistrationResult> CreateAsync(string email, string password)
    {
        var applicationUser = new ApplicationUser()
        {
            Email = email,
            UserName = email
        };

        var result = await userManager.CreateAsync(applicationUser, password);

        if(result.Succeeded)
        {
            return new UserRegistrationResult(true, applicationUser.Id, []);
        } else
        {
            return new UserRegistrationResult(false, null, result.Errors.Select(error => error.Description).ToList());
        }
    }
}