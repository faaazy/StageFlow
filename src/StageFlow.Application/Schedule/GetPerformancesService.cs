using StageFlow.Application.Festivals;
using StageFlow.Application.Festivals.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Application.Schedule;

public class GetPerformancesService(
    IPerformanceRepository performanceRepository,
    IFestivalRepository festivalRepository)
{
    public async Task<List<Performance>> GetAllAsync()
    {
        return await performanceRepository.GetAllAsync();
    }

    public async Task<List<Performance>> GetByFestivalAsync(Guid festivalId)
    {
        var festival = await festivalRepository.GetByIdAsync(festivalId) ?? 
            throw new FestivalNotFoundException();

        return await performanceRepository.GetByFestivalAsync(festival.Id);
    }
}