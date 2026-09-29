using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Persistence;
using StageFlow.Infrastructure.Schedule;
using Testcontainers.PostgreSql;

namespace StageFlow.IntegrationTests;

public class ScheduleRepositoryTests : IAsyncLifetime
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
    public async Task Should_detect_schedule_conflict()
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

        var startTime = new DateTimeOffset(2026, 09, 30, 10, 0, 0, TimeSpan.Zero);
        var endTime = new DateTimeOffset(2026, 09, 30, 12, 0, 0, TimeSpan.Zero);

        var performance = new Performance(stage.Id, artist.Id, startTime, endTime);

        _dbContext.Add(festival);
        _dbContext.Add(stage);
        _dbContext.Add(artist);
        _dbContext.Add(performance);

        await _dbContext.SaveChangesAsync();
        
        _dbContext.ChangeTracker.Clear();

        var schedule = new ScheduleRepository(_dbContext);

        var mockStartTime = new DateTimeOffset(2026, 09, 30, 11, 0, 0, TimeSpan.Zero);
        var mockEndTime = new DateTimeOffset(2026, 09, 30, 13, 0, 0, TimeSpan.Zero);

        var hasConflict = await schedule.HasScheduleConflict(stage.Id, mockStartTime, mockEndTime);

        hasConflict.Should().Be(true);
    }

    [Fact]
    public async Task Should_return_false_without_conflict()
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

        var startTime = new DateTimeOffset(2026, 09, 30, 10, 0, 0, TimeSpan.Zero);
        var endTime = new DateTimeOffset(2026, 09, 30, 12, 0, 0, TimeSpan.Zero);

        var performance = new Performance(stage.Id, artist.Id, startTime, endTime);

        _dbContext.Add(festival);
        _dbContext.Add(stage);
        _dbContext.Add(artist);
        _dbContext.Add(performance);

        await _dbContext.SaveChangesAsync();
        
        _dbContext.ChangeTracker.Clear();

        var schedule = new ScheduleRepository(_dbContext);

        var mockStartTime = new DateTimeOffset(2026, 09, 30, 12, 0, 0, TimeSpan.Zero);
        var mockEndTime = new DateTimeOffset(2026, 09, 30, 14, 0, 0, TimeSpan.Zero);

        var hasConflict = await schedule.HasScheduleConflict(stage.Id, mockStartTime, mockEndTime);

        hasConflict.Should().Be(false);
    }

    [Fact]
    public async Task Should_detect_conflict_when_new_performance_ends_during_existing_one()
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

        var startTime = new DateTimeOffset(2026, 09, 30, 10, 0, 0, TimeSpan.Zero);
        var endTime = new DateTimeOffset(2026, 09, 30, 12, 0, 0, TimeSpan.Zero);

        var performance = new Performance(stage.Id, artist.Id, startTime, endTime);

        _dbContext.Add(festival);
        _dbContext.Add(stage);
        _dbContext.Add(artist);
        _dbContext.Add(performance);

        await _dbContext.SaveChangesAsync();
        
        _dbContext.ChangeTracker.Clear();

        var schedule = new ScheduleRepository(_dbContext);

        var mockStartTime = new DateTimeOffset(2026, 09, 30, 9, 0, 0, TimeSpan.Zero);
        var mockEndTime = new DateTimeOffset(2026, 09, 30, 11, 0, 0, TimeSpan.Zero);

        var hasConflict = await schedule.HasScheduleConflict(stage.Id, mockStartTime, mockEndTime);

        hasConflict.Should().Be(true);
    }
}
