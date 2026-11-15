using StageFlow.Domain.Festival;

namespace StageFlow.Api.Contracts.Festivals;

public class ChangeFestivalStatusRequest
{
    public FestivalStatus Status { get; set; }
}