namespace StageFlow.Domain.Festival;

public class Performance
{
    public Guid Id {get; set;}

    public Guid StageId {get; set;}
    public Stage Stage {get; set;} = null!;

    public Guid ArtistId {get; set;}
    public Artist Artist {get; set;} = null!;

    public DateTimeOffset StartTime {get; set;}
    public DateTimeOffset EndTime {get; set;}

    public Performance (Guid stageId, Guid artistId, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        if(stageId == Guid.Empty)
            throw new ArgumentException("Stage id cannot be empty", nameof(stageId));

        if(artistId == Guid.Empty)
            throw new ArgumentException("Artist id cannot be empty", nameof(artistId));

        if(startTime >= endTime)
            throw new ArgumentException("Performance end time must be after start time.");

        Id = Guid.NewGuid();
        StageId = stageId;
        ArtistId = artistId;
        StartTime = startTime;
        EndTime = endTime;
    }

    private Performance() {}
}