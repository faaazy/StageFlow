using Microsoft.EntityFrameworkCore;
using StageFlow.Application.Schedule;
using StageFlow.Infrastructure.Persistence;

namespace StageFlow.Infrastructure.Schedule;

public class ScheduleRepository(AppDbContext dbContext) : IScheduleRepository
{
    public async Task<bool> HasScheduleConflict(
            Guid stageId,
            DateTimeOffset startTime,
            DateTimeOffset endTime
        )
    {
        return await dbContext.Performances
            .Where(p => p.StageId == stageId)
            .Where(p => p.EndTime > startTime && p.StartTime < endTime)
            .AnyAsync();
    }
}