namespace StageFlow.Domain.Festival;

public class Artist
{
    public Guid Id {get; private set;}
    public string Name {get; private set;} = "";

    public Artist(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Artist name cannot be empty.", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
    }

    private Artist() {}
}