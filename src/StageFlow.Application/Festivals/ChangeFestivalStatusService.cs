using StageFlow.Application.Festivals.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Application.Festivals;

public class ChangeFestivalStatusService(IFestivalRepository festivalRepository)
{
    public async Task<Festival> ChangeStatusAsync(Guid festivalId, Guid userId, FestivalStatus newStatus)
    {
        var festival = await festivalRepository.GetByIdAsync(festivalId)
            ?? throw new FestivalNotFoundException();

        if (festival.OrganizerId != userId) throw new FestivalAccessDeniedException();

        try
        {
            festival.ChangeStatus(newStatus);
        }
        catch (InvalidOperationException ex)
        {
            throw new FestivalStatusTransitionException(ex.Message);
        }

        await festivalRepository.UpdateAsync(festival);

        return festival;
    }
}