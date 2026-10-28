using StageFlow.Application.Festivals;
using StageFlow.Application.Festivals.Exceptions;
using StageFlow.Application.Stages.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Application.Stages;

public class CreateStageService(
    IFestivalRepository festivalRepository,
    IStageRepository stageRepository)
{
    public async Task<Stage> CreateAsync(Guid festivalId, Guid userId, string name)
    {
        var festival = await festivalRepository.GetByIdAsync(festivalId) 
                ?? throw new FestivalNotFoundException();

        if(festival.OrganizerId != userId) 
            throw new FestivalAccessDeniedException();

        Stage stage;

        try
        {
            stage = new Stage(name, festivalId);
        }
        catch (ArgumentException ex)
        {
            throw new StageValidationException(ex.Message);
        }

        return await stageRepository.AddAsync(stage);
    }
}