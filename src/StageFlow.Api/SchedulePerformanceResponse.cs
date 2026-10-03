namespace StageFlow.Api;

public class SchedulePerformanceResponse
{
    public Guid Id {get; set;}
    public Guid StageId {get; set;}
    public string StageName {get; set;} = "";

    public Guid ArtistId {get; set;}
    public string ArtistName {get; set;} = "";

    public DateTimeOffset StartTime {get; set;}
    public DateTimeOffset EndTime {get; set;}
}