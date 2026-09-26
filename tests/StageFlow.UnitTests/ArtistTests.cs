using FluentAssertions;
using StageFlow.Domain.Festival;

namespace StageFlow.UnitTests;

public class ArtistTests
{
    [Fact]
    public void ArtistWithValidName_ShouldSetName()
    {
        var artist = new Artist("Michael Jackson");

        artist.Name.Should().Be("Michael Jackson");
    }

    [Fact]
    public void ArtistWithInvalidName_ShouldThrow()
    {
        Action act = () => new Artist(" ");

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("name");
    }
}