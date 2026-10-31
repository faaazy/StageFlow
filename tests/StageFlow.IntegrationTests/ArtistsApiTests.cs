namespace StageFlow.IntegrationTests;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using StageFlow.IntegrationTests.Infrastructure;

public class ArtistsApiTests(StageFlowApiFactory factory)
    : IClassFixture<StageFlowApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private record RegisterResponse(Guid UserId);

    private record LoginResponse(string AccessToken);

    private record ArtistResponseDto(Guid Id, Guid OrganizerId, string Name);

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

    private static HttpRequestMessage CreateArtistRequest(string name, string? token)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/artists")
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
            CreateArtistRequest("Michael Jackson", token: null));

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_WithToken_ReturnsCreatedWithOrganizerFromToken()
    {
        var (userId, token) = await RegisterAndLoginAsync();

        var response = await _client.SendAsync(
            CreateArtistRequest("Michael Jackson", token));

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var artist = await response.Content.ReadFromJsonAsync<ArtistResponseDto>();

        artist.Should().NotBeNull();
        artist.OrganizerId.Should().Be(userId);
        artist.Name.Should().Be("Michael Jackson");
    }

    [Fact]
    public async Task Create_WithEmptyName_ReturnsBadRequest()
    {
        var (_, token) = await RegisterAndLoginAsync();

        var response = await _client.SendAsync(
            CreateArtistRequest("   ", token));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}