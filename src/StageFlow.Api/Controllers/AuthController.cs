using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StageFlow.Application.Auth;

namespace StageFlow.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    RegisterUserService registerUserService,
    LoginUserService loginUserService) 
    : ControllerBase
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await loginUserService.LoginAsync(
            request.Email,
            request.Password
        );

        return result.Succeeded 
            ? Ok(new { accessToken = result.Token }) 
            : Unauthorized(result.Errors);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetUserId()
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        return Ok(new {userId = sub});
    }
}