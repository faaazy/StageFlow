namespace StageFlow.Api.Contracts.Festivals;

public class UpdateFestivalRequest
{
    public string Name {get; set;} = "";

    public string? Description {get; set;} = "";

    public string Location {get; set;} = "";

    public DateTime StartDate {get; set;}

    public DateTime EndDate {get; set;}

    public string TimeZone {get;set;} = "";
}