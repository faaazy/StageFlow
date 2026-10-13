using Microsoft.AspNetCore.Mvc;
using StageFlow.Application.Auth;

namespace StageFlow.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(RegisterUserService registerUserService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await registerUserService.RegisterAsync(
            request.Email,
            request.Password
        );

        return result.Succeeded 
            ? StatusCode(StatusCodes.Status201Created, new { userId = result.UserId }) 
            : BadRequest(result.Errors);
    }
}