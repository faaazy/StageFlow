using FluentAssertions;
using StageFlow.Domain.Festival;

namespace StageFlow.UnitTests;

public class FestivalTests
{
    private static Festival CreateLiveFestival()
    {
        var festival = new Festival();

        festival.ChangeStatus(FestivalStatus.Published);
        festival.ChangeStatus(FestivalStatus.OnSale);
        festival.ChangeStatus(FestivalStatus.Live);

        return festival;
    }


    [Fact]
    public void DraftToPublished_ShouldChangeStatus()
    {
        var festival = new Festival();

        festival.ChangeStatus(FestivalStatus.Published);

        festival.Status.Should().Be(FestivalStatus.Published);
    }

    [Theory]
    [InlineData(FestivalStatus.Live)]
    [InlineData(FestivalStatus.OnSale)]
    public void DraftToInvalidStatus_ShouldThrow(FestivalStatus newStatus)
    {
        var festival = new Festival();

        Action act = () => festival.ChangeStatus(newStatus);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void FinishedToLive_ShouldThrow()
    {
        var festival = CreateLiveFestival();

        festival.ChangeStatus(FestivalStatus.Finished);

        Action act = () => festival.ChangeStatus(FestivalStatus.Live);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void LiveToFinished_ShouldChangeStatus()
    {
        var festival = CreateLiveFestival();

        festival.ChangeStatus(FestivalStatus.Finished);

        festival.Status.Should().Be(FestivalStatus.Finished);
    }

    [Fact]
    public void DraftToCancelled_ShouldChangeStatus()
    {
        var festival = new Festival();

        festival.ChangeStatus(FestivalStatus.Cancelled);

        festival.Status.Should().Be(FestivalStatus.Cancelled);
    }
}
