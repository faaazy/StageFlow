namespace StageFlow.Domain.Festival;

public class Artist
{
    public Guid Id {get; private set;}

    public Guid OrganizerId {get; private set;}
    public string Name {get; private set;} = "";

    public Artist(string name, Guid organizerId)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Artist name cannot be empty.", nameof(name));

        if(organizerId == Guid.Empty)
            throw new ArgumentException("Organizer ID cannot be empty", nameof(organizerId));

        Id = Guid.NewGuid();
        Name = name;
        OrganizerId = organizerId;
    }

    private Artist() {}
}