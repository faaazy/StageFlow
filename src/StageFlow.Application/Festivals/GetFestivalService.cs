using StageFlow.Domain.Festival;

namespace StageFlow.Application.Festivals;

public class GetFestivalService(IFestivalRepository festivalRepository)
{
    public async Task<Festival?> GetByIdAsync(Guid id)
    {
        return await festivalRepository.GetByIdAsync(id);
    }
}