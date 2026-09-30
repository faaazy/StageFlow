using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StageFlow.Application.Schedule;
using StageFlow.Application.Schedule.Exceptions;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Persistence;
using StageFlow.Infrastructure.Schedule;
using Testcontainers.PostgreSql;

namespace StageFlow.IntegrationTests;

public class CreatePerformanceServiceTests : IAsyncLifetime
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
    public async Task Service_creates_performance_if_no_conflict()
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

        _dbContext.Add(festival);
        _dbContext.Add(stage);
        _dbContext.Add(artist);

        await _dbContext.SaveChangesAsync();
        
        _dbContext.ChangeTracker.Clear();


        var scheduleRepo = new ScheduleRepository(_dbContext);
        var performanceRepo = new PerformanceRepository(_dbContext);

        var performanceService = new CreatePerformanceService(scheduleRepo, performanceRepo);

        var performance = await performanceService.CreateAsync(stage.Id, artist.Id, startTime, endTime);

        performance.StageId.Should().Be(stage.Id);
        performance.ArtistId.Should().Be(artist.Id);
        performance.StartTime.Should().Be(startTime);
        performance.EndTime.Should().Be(endTime);

        _dbContext.ChangeTracker.Clear();

        var result = await _dbContext.Performances.FirstAsync(p => p.Id == performance.Id);

        result.Id.Should().Be(performance.Id);
    }

    [Fact]
    public async Task Service_throws_when_schedule_conflicts()
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

        var existingPerformance = new Performance(
            stage.Id,
            artist.Id,
            startTime,
            endTime);

        _dbContext.Add(festival);
        _dbContext.Add(stage);
        _dbContext.Add(artist);
        _dbContext.Add(existingPerformance);

        await _dbContext.SaveChangesAsync();
        
        _dbContext.ChangeTracker.Clear();


        var scheduleRepo = new ScheduleRepository(_dbContext);
        var performanceRepo = new PerformanceRepository(_dbContext);

        var performanceService = new CreatePerformanceService(scheduleRepo, performanceRepo);

        var wrongStartTime = new DateTimeOffset(2026, 09, 30, 11, 0, 0, TimeSpan.Zero);
        var wrongEndTime = new DateTimeOffset(2026, 09, 30, 13, 0, 0, TimeSpan.Zero);

        Func<Task> act = () => performanceService.CreateAsync(
                stage.Id, artist.Id, wrongStartTime, wrongEndTime
            );

        await act.Should().ThrowAsync<PerformanceScheduleConflictException>();

        var count = await _dbContext.Performances.CountAsync(p => p.StageId == stage.Id);

        count.Should().Be(1);
    }
}
