using Microsoft.EntityFrameworkCore;
using StageFlow.Application.Festivals;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Persistence;

namespace StageFlow.Infrastructure.Festivals;

public class FestivalRepository(AppDbContext dbContext) : IFestivalRepository
{
    public async Task<Festival> AddAsync(Festival festival)
    {
        await dbContext.Festivals.AddAsync(festival);

        await dbContext.SaveChangesAsync();

        return festival;
    }

    public async Task<Festival?> GetByIdAsync(Guid id)
    {
        return await dbContext.Festivals.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task UpdateAsync(Festival festival)
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Festival>> GetByOrganizerAsync(Guid organizerId)
    {
        return await dbContext.Festivals
            .Where(f => f.OrganizerId == organizerId)
            .OrderBy(f => f.StartDate)
            .AsNoTracking()
            .ToListAsync();
    }
}