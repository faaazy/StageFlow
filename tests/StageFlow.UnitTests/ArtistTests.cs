using FluentAssertions;
using StageFlow.Domain.Festival;

namespace StageFlow.UnitTests;

public class ArtistTests
{
    [Fact]
    public void ArtistWithValidName_ShouldSetName()
    {
        var artist = new Artist("Michael Jackson", Guid.NewGuid());

        artist.Name.Should().Be("Michael Jackson");
    }

    [Fact]
    public void ArtistWithInvalidName_ShouldThrow()
    {
        Action act = () => new Artist(" ", Guid.NewGuid());

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("name");
    }

    [Fact]
    public void ArtistWithoutOrganizerId_ShouldThrow()
    {
        Action act = () => new Artist("Michael Jackson", Guid.Empty);

        var exception = act.Should().Throw<ArgumentException>().Which;

        exception.ParamName.Should().Be("organizerId");
    }

    [Fact]
    public void ArtistWithValidOrganizerId_ShouldSetOrganizerId()
    {
        var organizerId = Guid.NewGuid();

        var artist = new Artist("Michael Jackson", organizerId);

        artist.OrganizerId.Should().Be(organizerId);
    }
}