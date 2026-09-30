using StageFlow.Application.Schedule.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Application.Schedule;

public class CreatePerformanceService(
    IScheduleRepository scheduleRepository,
    IPerformanceRepository performanceRepository)
{
    public async Task<Performance> CreateAsync(
            Guid stageId, Guid artistId, 
            DateTimeOffset startTime, DateTimeOffset endTime
        )
    {
        var hasConflict = await scheduleRepository.HasScheduleConflict(stageId, startTime, endTime);

        if(hasConflict) throw new PerformanceScheduleConflictException();

        var performance = new Performance(stageId, artistId, startTime, endTime);

        return await performanceRepository.AddAsync(performance);
    }
}