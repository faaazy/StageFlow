using StageFlow.Domain.Festival;

namespace StageFlow.Application.Festivals;

public interface IFestivalRepository
{
    Task<Festival> AddAsync(Festival festival);
}