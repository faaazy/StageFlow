using StageFlow.Application.Artists.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Application.Artists;

public class CreateArtistService (IArtistRepository artistRepository)
{
    public async Task<Artist> CreateAsync(Guid organizerId, string name)
    {
        Artist artist;

        try
        {
            artist = new Artist(name, organizerId);
        }
        catch (ArgumentException ex)
        {
            throw new ArtistValidationException(ex.Message);
        }

        return await artistRepository.AddAsync(artist);
    }
}