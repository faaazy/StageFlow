using StageFlow.Application.Artists.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Application.Artists;

public class GetArtistsService(IArtistRepository artistRepository)
{
    public async Task<List<Artist>> GetByOrganizerAsync(Guid organizerId)
    {
        return await artistRepository.GetByOrganizerAsync(organizerId);
    }

    public async Task<Artist> GetByIdAsync(Guid id, Guid userId)
    {
        var artist = await artistRepository.GetByIdAsync(id);

        if(artist is null || artist.OrganizerId != userId)
            throw new ArtistNotFoundException(); 

        return artist;
    }
}