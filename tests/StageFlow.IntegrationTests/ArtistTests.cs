using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace StageFlow.IntegrationTests;

public class ArtistTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:18")
        .WithDatabase("stageflow_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private AppDbContext _dbContext = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseNpgsql(_postgres.GetConnectionString());

        _dbContext = new AppDbContext(optionsBuilder.Options);

        await _dbContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task Should_save_and_load_artist()
    {
        var artist = new Artist("Michael Jackson");

        _dbContext.Add(artist);

        await _dbContext.SaveChangesAsync();

        _dbContext.ChangeTracker.Clear();

        var savedArtist = await _dbContext.Artists
            .FirstAsync(a => a.Id == artist.Id);
        
        savedArtist.Id.Should().Be(artist.Id);
        savedArtist.Name.Should().Be("Michael Jackson");        
    }
}
