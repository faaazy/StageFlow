namespace StageFlow.Api.Contracts.Stages;

public class StageResponse
{
    public Guid Id {get; set;}

    public Guid FestivalId {get; set;}

    public string Name {get; set;} = "";
}