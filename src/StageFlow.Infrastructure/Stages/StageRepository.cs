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
}