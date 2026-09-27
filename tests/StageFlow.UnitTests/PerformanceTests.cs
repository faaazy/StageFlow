using FluentAssertions;
using StageFlow.Domain.Festival;

namespace StageFlow.UnitTests;

public class PerformanceTests
{
    [Fact]
    public void PerformanceWithValidData_ShouldSetProperties()
    {
        var startTime = new DateTimeOffset(2026, 9, 27, 18, 0, 0, TimeSpan.FromHours(3));
        var endTime = new DateTimeOffset(2026, 9, 27, 20, 0, 0, TimeSpan.FromHours(3));
        var artistId = Guid.NewGuid();
        var stageId = Guid.NewGuid();

        var performance = new Performance(stageId, artistId, startTime, endTime);

        performance.Id.Should().NotBe(Guid.Empty);
        performance.StartTime.Should().Be(startTime);
        performance.EndTime.Should().Be(endTime);
        performance.ArtistId.Should().Be(artistId);
        performance.StageId.Should().Be(stageId);
    }

    [Fact]
    public void PerformanceWithInvalidStageId_ShouldThrow()
    {
        var startTime = new DateTimeOffset(2026, 9, 27, 18, 0, 0, TimeSpan.FromHours(3));
        var endTime = new DateTimeOffset(2026, 9, 27, 20, 0, 0, TimeSpan.FromHours(3));
        var artistId = Guid.NewGuid();
        var stageId = Guid.Empty;

        Action act = () => new Performance(stageId, artistId, startTime, endTime);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("stageId");
    }

    [Fact]
    public void PerformanceWithInvalidArtistId_ShouldThrow()
    {
        var startTime = new DateTimeOffset(2026, 9, 27, 18, 0, 0, TimeSpan.FromHours(3));
        var endTime = new DateTimeOffset(2026, 9, 27, 20, 0, 0, TimeSpan.FromHours(3));
        var artistId = Guid.Empty;
        var stageId = Guid.NewGuid();

        Action act = () => new Performance(stageId, artistId, startTime, endTime);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("artistId");
    }

    [Fact]
    public void PerformanceWithInvalidTime_ShouldThrow()
    {
        var startTime = new DateTimeOffset(2026, 9, 27, 18, 0, 0, TimeSpan.FromHours(3));
        var endTime = new DateTimeOffset(2026, 9, 27, 18, 0, 0, TimeSpan.FromHours(3));
        var artistId = Guid.NewGuid();
        var stageId = Guid.NewGuid();

        Action act = () => new Performance(stageId, artistId, startTime, endTime);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void PerformanceEndingBeforeStarting_ShouldThrow()
    {
        var startTime = new DateTimeOffset(2026, 9, 27, 20, 0, 0, TimeSpan.FromHours(3));
        var endTime = new DateTimeOffset(2026, 9, 27, 18, 0, 0, TimeSpan.FromHours(3));
        var artistId = Guid.NewGuid();
        var stageId = Guid.NewGuid();

        Action act = () => new Performance(stageId, artistId, startTime, endTime);

        act.Should().Throw<ArgumentException>();
    }
}