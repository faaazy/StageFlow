using StageFlow.Domain.Festival;

namespace StageFlow.Application.Schedule;

public interface IPerformanceRepository
{
    Task<Performance> AddAsync(Performance performance);
}