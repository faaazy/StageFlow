using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using StageFlow.IntegrationTests.Infrastructure;

namespace StageFlow.IntegrationTests;

public class StagesApiTests(StageFlowApiFactory factory)
    : IClassFixture<StageFlowApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private record RegisterResponse(Guid UserId);

    private record LoginResponse(string AccessToken);

    private record FestivalResponseDto(Guid Id);

    private record StageResponseDto(Guid Id, Guid FestivalId, string Name);

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

    private async Task<Guid> CreateFestivalAsync(string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/festivals")
        {
            Content = JsonContent.Create(new
            {
                Name = "Test Fest",
                Location = "Moscow",
                StartDate = DateTime.UtcNow.AddDays(30),
                EndDate = DateTime.UtcNow.AddDays(32),
                TimeZone = "Europe/Moscow"
            })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        var festival = await response.Content.ReadFromJsonAsync<FestivalResponseDto>();

        return festival!.Id;
    }

    private static HttpRequestMessage CreateStageRequest(
        Guid festivalId, string name, string? token)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post, $"/festivals/{festivalId}/stages")
        {
            Content = JsonContent.Create(new { Name = name })
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
        var response = await _client.SendAsync(
            CreateStageRequest(Guid.NewGuid(), "Main Stage", token: null));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_ByOrganizer_ReturnsCreatedStage()
    {
        var token = await RegisterAndLoginAsync();
        var festivalId = await CreateFestivalAsync(token);

        var response = await _client.SendAsync(
            CreateStageRequest(festivalId, "Main Stage", token));

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var stage = await response.Content.ReadFromJsonAsync<StageResponseDto>();

        stage.Should().NotBeNull();
        stage.FestivalId.Should().Be(festivalId);
        stage.Name.Should().Be("Main Stage");
    }

    [Fact]
    public async Task Create_ByAnotherUser_ReturnsForbidden()
    {
        var ownerToken = await RegisterAndLoginAsync();
        var strangerToken = await RegisterAndLoginAsync();
        var festivalId = await CreateFestivalAsync(ownerToken);

        var response = await _client.SendAsync(
            CreateStageRequest(festivalId, "Main Stage", strangerToken));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_ForUnknownFestival_ReturnsNotFound()
    {
        var token = await RegisterAndLoginAsync();


        var response = await _client.SendAsync(
            CreateStageRequest(Guid.NewGuid(), "Main Stage", token));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_WithEmptyName_ReturnsBadRequest()
    {
        var token = await RegisterAndLoginAsync();
        var festivalId = await CreateFestivalAsync(token);

        var response = await _client.SendAsync(
            CreateStageRequest(festivalId, "   ", token));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}