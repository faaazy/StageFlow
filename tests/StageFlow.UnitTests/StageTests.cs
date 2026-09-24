using FluentAssertions;
using StageFlow.Domain.Festival;

namespace StageFlow.UnitTests;

public class StageTests
{
    [Fact]
    public void StageWithValidName_ShouldSetName()
    {
        var stage = new Stage("Main Stage", Guid.NewGuid());

        stage.Name.Should().Be("Main Stage");
    }

    [Fact]
    public void StageWithInvalidName_ShouldThrow()
    {
        Action act = () => new Stage("  ", Guid.NewGuid());

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("name");
    }

    [Fact]
    public void StageWithInvalidFestivalId_ShouldThrow()
    {
        Action act = () => new Stage("Main Stage", Guid.Empty);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("festivalId");
    }
}