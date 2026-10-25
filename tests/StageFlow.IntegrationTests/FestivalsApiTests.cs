using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using StageFlow.IntegrationTests.Infrastructure;

namespace StageFlow.IntegrationTests;

public class FestivalsApiTests(StageFlowApiFactory factory)
    : IClassFixture<StageFlowApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private record RegisterResponse(Guid UserId);

    private record LoginResponse(string AccessToken);

    private record FestivalResponseDto(Guid Id, Guid OrganizerId);

    private async Task<(Guid UserId, string Token)> RegisterAndLoginAsync()
    {
        var email = $"test-{Guid.NewGuid():N}@example.com";
        var password = "StrongPass123!";

        var registerResponse = await _client.PostAsJsonAsync(
            "/auth/register", new { Email = email, Password = password });
        var registered = await registerResponse.Content
            .ReadFromJsonAsync<RegisterResponse>();

        var loginResponse = await _client.PostAsJsonAsync(
            "/auth/login", new { Email = email, Password = password });
        var login = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        return (registered!.UserId, login!.AccessToken);
    }

    private static object ValidFestivalBody() => new
    {
        Name = "Test Fest",
        Location = "Moscow",
        StartDate = DateTime.UtcNow.AddDays(30),
        EndDate = DateTime.UtcNow.AddDays(32),
        TimeZone = "Europe/Moscow"
    };

    private static HttpRequestMessage CreateFestivalRequest(object body, string? token)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/festivals")
        {
            Content = JsonContent.Create(body)
        };

        if (token is not null)
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return request;
    }

    [Fact]
    public async Task Create_WithoutToken_ReturnsUnauthorized()
    {
        var request = CreateFestivalRequest(ValidFestivalBody(), token: null);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_WithToken_ReturnsCreatedWithOrganizerFromToken()
    {
        var (userId, token) = await RegisterAndLoginAsync();

        var request = CreateFestivalRequest(ValidFestivalBody(), token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var festival = await response.Content
            .ReadFromJsonAsync<FestivalResponseDto>();

        festival.Should().NotBeNull();
        festival.OrganizerId.Should().Be(userId);
    }

    [Fact]
    public async Task Create_WithEndBeforeStart_ReturnsBadRequest()
    {
        var (_, token) = await RegisterAndLoginAsync();

        var body = new
        {
            Name = "Test Fest",
            Location = "Moscow",
            StartDate = DateTime.UtcNow.AddDays(32),
            EndDate = DateTime.UtcNow.AddDays(30),
            TimeZone = "Europe/Moscow"
        };

        var request = CreateFestivalRequest(body, token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}