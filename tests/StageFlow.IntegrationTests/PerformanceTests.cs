using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace StageFlow.IntegrationTests;

public class PerformanceTests : IAsyncLifetime
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
    public async Task Should_save_and_load_performance()
    {
        var festival = new Festival
        {
            Id = Guid.NewGuid(),
            Name = "Test Festival",
            Location = "Tallinn",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(2),
            TimeZone = "Europe/Tallinn"
        };

        var stage = new Stage("Main Stage", festival.Id);

        var artist = new Artist("Michael Jackson");

        var startTime = new DateTimeOffset(2026, 09, 27, 18, 0, 0, TimeSpan.Zero);
        var endTime = new DateTimeOffset(2026, 09, 27, 20, 0, 0, TimeSpan.Zero);

        var performance = new Performance(stage.Id, artist.Id, startTime, endTime);

        _dbContext.Add(festival);
        _dbContext.Add(stage);
        _dbContext.Add(artist);
        _dbContext.Add(performance);

        await _dbContext.SaveChangesAsync();
        
        _dbContext.ChangeTracker.Clear();

        var result = await _dbContext.Performances
            .Include(p => p.Artist)
            .Include(p => p.Stage)
            .FirstAsync(p => p.Id == performance.Id);

        result.Artist.Name.Should().Be("Michael Jackson");
        result.Stage.Name.Should().Be("Main Stage");

        result.ArtistId.Should().Be(artist.Id);
        result.StageId.Should().Be(stage.Id);

        result.StartTime.Should().Be(startTime);
        result.EndTime.Should().Be(endTime);
    }
}