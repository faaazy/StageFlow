using StageFlow.Application.Artists;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Persistence;

namespace StageFlow.Infrastructure.Artists;

public class ArtistRepository(AppDbContext dbContext) : IArtistRepository
{
    public async Task<Artist> AddAsync(Artist artist)
    {
        await dbContext.Artists.AddAsync(artist);

        await dbContext.SaveChangesAsync();

        return artist;
    }
}