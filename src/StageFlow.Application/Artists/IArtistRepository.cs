using StageFlow.Domain.Festival;

namespace StageFlow.Application.Artists;

public interface IArtistRepository
{
    Task<Artist> AddAsync(Artist artist);

    Task<Artist?> GetByIdAsync(Guid id);

    Task<List<Artist>> GetByOrganizerAsync(Guid organizerId);
}