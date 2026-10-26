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

    private static HttpRequestMessage GetFestivalRequest(Guid id, string? token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/festivals/{id}");

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

    [Fact]
    public async Task Create_WithDateWithoutTimezone_ReturnsBadRequest()
    {
        var (_, token) = await RegisterAndLoginAsync();

        var json = """
        {
            "name": "Test Fest",
            "location": "Moscow",
            "startDate": "2026-11-01T10:00:00",
            "endDate": "2026-11-03T10:00:00",
            "timeZone": "Europe/Moscow"
        }
        """;

        var request = new HttpRequestMessage(HttpMethod.Post, "/festivals")
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.SendAsync(
            GetFestivalRequest(Guid.NewGuid(), token: null));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_ExistingFestival_ReturnsOk()
    {
        var (_, token) = await RegisterAndLoginAsync();

        var createResponse = await _client.SendAsync(
            CreateFestivalRequest(ValidFestivalBody(), token));
        var created = await createResponse.Content
            .ReadFromJsonAsync<FestivalResponseDto>();

        var response = await _client.SendAsync(
            GetFestivalRequest(created!.Id, token));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var festival = await response.Content
            .ReadFromJsonAsync<FestivalResponseDto>();

        festival!.Id.Should().Be(created.Id);
    }

    [Fact]
    public async Task Get_UnknownFestival_ReturnsNotFound()
    {
        var (_, token) = await RegisterAndLoginAsync();

        var response = await _client.SendAsync(
            GetFestivalRequest(Guid.NewGuid(), token));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}