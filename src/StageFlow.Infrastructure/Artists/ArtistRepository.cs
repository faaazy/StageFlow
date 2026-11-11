using Microsoft.EntityFrameworkCore;
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

    public async Task<Artist?> GetByIdAsync(Guid id)
    {
        return await dbContext.Artists.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Artist>> GetByOrganizerAsync(Guid organizerId)
    {
        return await dbContext.Artists
            .Where(a => a.OrganizerId == organizerId)
            .OrderBy(a => a.Name)
            .AsNoTracking()
            .ToListAsync();
    }
}