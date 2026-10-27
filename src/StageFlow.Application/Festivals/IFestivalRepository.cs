using StageFlow.Domain.Festival;

namespace StageFlow.Application.Festivals;

public interface IFestivalRepository
{
    Task<Festival> AddAsync(Festival festival);

    Task<Festival?> GetByIdAsync(Guid id);

    Task UpdateAsync(Festival festival);
}