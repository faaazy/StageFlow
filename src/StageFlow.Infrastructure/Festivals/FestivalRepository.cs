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
}