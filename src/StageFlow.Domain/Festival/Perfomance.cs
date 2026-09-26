namespace StageFlow.Domain.Festival;

public class Perfomance
{
    public Guid Id {get; set;}

    public Guid StageId {get; set;}

    public Stage Stage {get; set;} = null!;

    public Guid ArtistId {get; set;}
    // TODO: add artist entity

    public DateTimeOffset StartTime {get; set;}
    public DateTimeOffset EndTime {get; set;}
}