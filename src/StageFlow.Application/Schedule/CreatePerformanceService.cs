using StageFlow.Application.Festivals;
using StageFlow.Application.Festivals.Exceptions;
using StageFlow.Application.Schedule.Exceptions;
using StageFlow.Application.Stages;
using StageFlow.Application.Stages.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Application.Schedule;

public class CreatePerformanceService(
    IScheduleRepository scheduleRepository,
    IPerformanceRepository performanceRepository,
    IStageRepository stageRepository,
    IFestivalRepository festivalRepository)
{
    public async Task<Performance> CreateAsync(
        Guid userId,
        Guid stageId, 
        Guid artistId, 
        DateTimeOffset startTime, 
        DateTimeOffset endTime)
    {
        var stage = await stageRepository.GetByIdAsync(stageId) ?? throw new StageNotFoundException();
        var festival = await festivalRepository.GetByIdAsync(stage.FestivalId) ?? throw new FestivalNotFoundException();

        if(festival.OrganizerId != userId) throw new FestivalAccessDeniedException();

        var hasConflict = await scheduleRepository.HasScheduleConflict(stageId, startTime, endTime);

        if(hasConflict) throw new PerformanceScheduleConflictException();

        var performance = new Performance(stageId, artistId, startTime, endTime);

        return await performanceRepository.AddAsync(performance);
    }
}