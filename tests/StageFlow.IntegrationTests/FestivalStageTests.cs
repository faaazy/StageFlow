using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace StageFlow.IntegrationTests;

public class FestivalStageTests : IAsyncLifetime
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
    public async Task Should_load_stages_for_festival()
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

        var stage = new Stage("Main Stage", Guid.NewGuid())
        {
            Festival = festival,
            FestivalId = festival.Id
        };

        festival.Stages.Add(stage);

        _dbContext.Add(festival);

        await _dbContext.SaveChangesAsync();

        _dbContext.ChangeTracker.Clear();

        var savedStage = await _dbContext.Stages
            .FirstAsync(s => s.Id == stage.Id);
        
        savedStage.FestivalId.Should().Be(festival.Id);

        var result = await _dbContext.Festivals
            .Include(f => f.Stages)
            .FirstAsync(f => f.Id == festival.Id);

        result.Stages.Should().ContainSingle();
        result.Stages.First().Name.Should().Be("Main Stage");
    }
}
