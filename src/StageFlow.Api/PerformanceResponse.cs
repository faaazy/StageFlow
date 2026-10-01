namespace StageFlow.Api;

public class PerformanceResponse
{
    public Guid Id {get; set;}
    public Guid StageId {get; set;}
    public Guid ArtistId {get; set;}
    public DateTimeOffset StartTime {get; set;}
    public DateTimeOffset EndTime {get; set;}
}