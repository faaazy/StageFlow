using StageFlow.Application.Festivals;
using StageFlow.Application.Festivals.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Application.Stages;

public class GetStagesService (
    IFestivalRepository festivalRepository, 
    IStageRepository stageRepository)
{
    public async Task<List<Stage>> GetByFestivalAsync(Guid festivalId)
    {
        var festival = await festivalRepository.GetByIdAsync(festivalId) 
            ?? throw new FestivalNotFoundException();

        return await stageRepository.GetByFestivalAsync(festival.Id);
    }
}