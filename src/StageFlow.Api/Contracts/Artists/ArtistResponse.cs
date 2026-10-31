namespace StageFlow.Api.Contracts.Artists;

public class ArtistResponse
{
    public Guid Id {get; set;}

    public Guid OrganizerId {get; set;}

    public string Name {get; set;} = "";
}