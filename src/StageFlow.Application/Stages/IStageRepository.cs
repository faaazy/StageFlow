using StageFlow.Domain.Festival;

namespace StageFlow.Application.Stages;

public interface IStageRepository
{
    Task<Stage> AddAsync(Stage stage);
}