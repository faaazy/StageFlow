using StageFlow.Domain.Festival;

namespace StageFlow.Application.Schedule;

public class GetPerformancesService(IPerformanceRepository performanceRepository)
{
    public async Task<List<Performance>> GetAllAsync()
    {
        return await performanceRepository.GetAllAsync();
    }
}