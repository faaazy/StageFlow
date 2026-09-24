namespace StageFlow.Domain.Festival;

public class Stage
{
    public Guid Id {get; private set;}

    public Guid FestivalId {get; set;}
    public Festival Festival {get; set;} = null!;

    public Stage(string name, Guid festivalId)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Stage name cannot be empty.", nameof(name));

        if(festivalId == Guid.Empty)
            throw new ArgumentException("Festival ID cannot be empty.", nameof(festivalId));

        Id = Guid.NewGuid();
        Name = name;
        FestivalId = festivalId;
    }

    private Stage() { }
    public string Name {get; private set;} = "";

    public void Rename(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Stage name cannot be empty.", nameof(name));

        Name = name;
    }
}