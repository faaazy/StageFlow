namespace StageFlow.Domain.Festival;

public class Stage
{
    public Guid Id {get; set;}

    public Guid FestivalId {get; set;}
    public Festival Festival {get; set;} = null!;

    public string Name {get; set;} = "";

}