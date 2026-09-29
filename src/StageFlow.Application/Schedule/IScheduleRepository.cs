namespace StageFlow.Application.Schedule; 

public interface IScheduleRepository
{
    Task<bool> HasScheduleConflict(
        Guid stageId, 
        DateTimeOffset startTime, 
        DateTimeOffset endTime
    );
}