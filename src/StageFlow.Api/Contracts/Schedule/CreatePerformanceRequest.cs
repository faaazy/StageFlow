namespace StageFlow.Api.Contracts.Schedule;

public class CreatePerformanceRequest
{
    public Guid StageId {get; set;}
    public Guid ArtistId {get; set;}
    public DateTimeOffset StartTime {get; set;}
    public DateTimeOffset EndTime {get; set;}
}