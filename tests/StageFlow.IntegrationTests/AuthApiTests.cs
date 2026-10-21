using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using StageFlow.IntegrationTests.Infrastructure;

namespace StageFlow.IntegrationTests;

public class AuthApiTests(StageFlowApiFactory factory)
    : IClassFixture<StageFlowApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private record RegisterResponse(Guid UserId);

    private record LoginResponse(string AccessToken);

    [Fact]
    public async Task Register_WithValidData_ReturnsCreated()
    {

        var request = new
        {
            Email = $"test-{Guid.NewGuid():N}@example.com",
            Password = "StrongPass123!"
        };

        var response = await _client.PostAsJsonAsync(
            "/auth/register",
            request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<RegisterResponse>();

        body.Should().NotBeNull();
        body.UserId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Register_WithShortPassword_ReturnsBadRequest()
    {
        var request = new
        {
            Email = $"test-{Guid.NewGuid():N}@example.com",
            Password = "123"
        };

        var response = await _client.PostAsJsonAsync(
            "/auth/register",
            request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsBadRequest()
    {
        var request = new
        {
            Email = $"test-{Guid.NewGuid():N}@example.com",
            Password = "hardPassword123!"
        };

        var response = await _client.PostAsJsonAsync(
            "/auth/register",
            request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var duplicateResponse = await _client.PostAsJsonAsync(
            "/auth/register",
            request);

        duplicateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    
    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        var email = $"test-{Guid.NewGuid():N}@example.com";
        var password = "StrongPass123!";

        var registerResponse = await _client.PostAsJsonAsync(
            "/auth/register",
            new { Email = email, Password = password });

        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var response = await _client.PostAsJsonAsync(
            "/auth/login",
            new { Email = email, Password = password });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsJwtForRegisteredUser()
    {
        var email = $"test-{Guid.NewGuid():N}@example.com";
        var password = "StrongPass123!";

        var registerResponse = await _client.PostAsJsonAsync(
            "/auth/register",
            new { Email = email, Password = password });

        var registered = await registerResponse.Content
            .ReadFromJsonAsync<RegisterResponse>();

        var response = await _client.PostAsJsonAsync(
            "/auth/login",
            new { Email = email, Password = password });

        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();

        body.Should().NotBeNull();
        body.AccessToken.Should().NotBeNullOrWhiteSpace();

        var token = new JsonWebTokenHandler().ReadJsonWebToken(body.AccessToken);

        token.Subject.Should().Be(registered!.UserId.ToString());
        token.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task Login_WithIncorrectPassword_ReturnsUnauthorized()
    {
        var email = $"test-{Guid.NewGuid():N}@example.com";
        var password = "StrongPass123!";

        var registerResponse = await _client.PostAsJsonAsync(
            "/auth/register",
            new { Email = email, Password = password });

        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var response = await _client.PostAsJsonAsync(
            "/auth/login",
            new { Email = email, Password = "WrongPass123!" });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithUnknownEmail_ReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync(
            "/auth/login",
            new
            {
                Email = $"unknown-{Guid.NewGuid():N}@example.com",
                Password = "StrongPass123!"
            });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
