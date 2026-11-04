using Microsoft.EntityFrameworkCore;
using StageFlow.Application.Stages;
using StageFlow.Domain.Festival;
using StageFlow.Infrastructure.Persistence;

namespace StageFlow.Infrastructure.Stages;

public class StageRepository(AppDbContext dbContext) : IStageRepository
{
    public async Task<Stage> AddAsync(Stage stage)
    {
        await dbContext.Stages.AddAsync(stage);

        await dbContext.SaveChangesAsync();

        return stage;
    }

    public async Task<Stage?> GetByIdAsync(Guid id)
    {
        return await dbContext.Stages.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Stage>> GetByFestivalAsync(Guid festivalId)
    {
        return await dbContext.Stages
            .Where(s => s.FestivalId == festivalId)
            .OrderBy(s => s.Name)
            .AsNoTracking()
            .ToListAsync();
    }
}