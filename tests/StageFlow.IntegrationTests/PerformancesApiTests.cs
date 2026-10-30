using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Persistence;
using StageFlow.IntegrationTests.Infrastructure;

namespace StageFlow.IntegrationTests;

public class PerformancesApiTests(StageFlowApiFactory factory)
    : IClassFixture<StageFlowApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private record LoginResponse(string AccessToken);

    private record IdResponse(Guid Id);

    private async Task<string> RegisterAndLoginAsync()
    {
        var email = $"test-{Guid.NewGuid():N}@example.com";
        var password = "StrongPass123!";

        await _client.PostAsJsonAsync(
            "/auth/register", new { Email = email, Password = password });

        var loginResponse = await _client.PostAsJsonAsync(
            "/auth/login", new { Email = email, Password = password });
        var login = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        return login!.AccessToken;
    }

    private static HttpRequestMessage Authorized(HttpMethod method, string url, object? body, string? token)
    {
        var request = new HttpRequestMessage(method, url);

        if (body is not null)
            request.Content = JsonContent.Create(body);

        if (token is not null)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return request;
    }

    private async Task<Guid> CreateFestivalAsync(string token)
    {
        var response = await _client.SendAsync(Authorized(HttpMethod.Post, "/festivals", new
        {
            Name = "Test Fest",
            Location = "Moscow",
            StartDate = DateTime.UtcNow.AddDays(30),
            EndDate = DateTime.UtcNow.AddDays(32),
            TimeZone = "Europe/Moscow"
        }, token));

        return (await response.Content.ReadFromJsonAsync<IdResponse>())!.Id;
    }

    private async Task<Guid> CreateStageAsync(Guid festivalId, string token)
    {
        var response = await _client.SendAsync(Authorized(
            HttpMethod.Post, $"/festivals/{festivalId}/stages",
            new { Name = "Main Stage" }, token));

        return (await response.Content.ReadFromJsonAsync<IdResponse>())!.Id;
    }

    private async Task<Guid> CreateArtistAsync()
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var artist = new Artist("Test Artist");
        dbContext.Artists.Add(artist);
        await dbContext.SaveChangesAsync();

        return artist.Id;
    }

    private static object PerformanceBody(Guid stageId, Guid artistId, int startHour = 12)
    {
        var day = DateTime.UtcNow.Date.AddDays(31);

        return new
        {
            StageId = stageId,
            ArtistId = artistId,
            StartTime = new DateTimeOffset(day.AddHours(startHour), TimeSpan.Zero),
            EndTime = new DateTimeOffset(day.AddHours(startHour + 1), TimeSpan.Zero)
        };
    }

    [Fact]
    public async Task Create_WithoutToken_ReturnsUnauthorized()
    {
        var body = PerformanceBody(Guid.NewGuid(), Guid.NewGuid());

        var response = await _client.SendAsync(
            Authorized(HttpMethod.Post, "/schedule/performances", body, token: null));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_ByOrganizer_ReturnsCreated()
    {
        var token = await RegisterAndLoginAsync();
        var stageId = await CreateStageAsync(await CreateFestivalAsync(token), token);
        var artistId = await CreateArtistAsync();

        var response = await _client.SendAsync(Authorized(
            HttpMethod.Post, "/schedule/performances",
            PerformanceBody(stageId, artistId), token));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Create_ByAnotherUser_ReturnsForbidden()
    {
        var ownerToken = await RegisterAndLoginAsync();
        var strangerToken = await RegisterAndLoginAsync();
        var stageId = await CreateStageAsync(await CreateFestivalAsync(ownerToken), ownerToken);
        var artistId = await CreateArtistAsync();

        var response = await _client.SendAsync(Authorized(
            HttpMethod.Post, "/schedule/performances",
            PerformanceBody(stageId, artistId), strangerToken));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_OnUnknownStage_ReturnsNotFound()
    {
        var token = await RegisterAndLoginAsync();
        var artistId = await CreateArtistAsync();

        var response = await _client.SendAsync(Authorized(
            HttpMethod.Post, "/schedule/performances",
            PerformanceBody(Guid.NewGuid(), artistId), token));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithOverlappingTime_ReturnsConflict()
    {
        var token = await RegisterAndLoginAsync();
        var stageId = await CreateStageAsync(await CreateFestivalAsync(token), token);
        var artistId = await CreateArtistAsync();

        await _client.SendAsync(Authorized(
            HttpMethod.Post, "/schedule/performances",
            PerformanceBody(stageId, artistId), token));

        var response = await _client.SendAsync(Authorized(
            HttpMethod.Post, "/schedule/performances",
            PerformanceBody(stageId, artistId), token));

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task GetAll_WithoutToken_StillReturnsOk()
    {
        var response = await _client.GetAsync("/schedule/performances");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Create_WithNonUtcOffset_ReturnsCreated()
    {
        var token = await RegisterAndLoginAsync();
        var stageId = await CreateStageAsync(await CreateFestivalAsync(token), token);
        var artistId = await CreateArtistAsync();

        var json = $$"""
        {
            "stageId": "{{stageId}}",
            "artistId": "{{artistId}}",
            "startTime": "2030-06-01T15:00:00+03:00",
            "endTime": "2030-06-01T16:00:00+03:00"
        }
        """;

        var request = new HttpRequestMessage(HttpMethod.Post, "/schedule/performances")
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}