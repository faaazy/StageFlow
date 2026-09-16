namespace StageFlow.Domain.Festival;

public class Festival
{
    public Guid Id {get; set;}

    public string Name {get; set;} = "";

    public string? Description {get; set;} = "";

    public string Location {get; set;} = "";

    public DateTime StartDate {get; set;}

    public DateTime EndDate {get; set;}

    public string TimeZone {get;set;} = "";

    public FestivalStatus Status {get; private set;}

    public void ChangeStatus(FestivalStatus newStatus)
    {
        switch (Status)
        {
            case FestivalStatus.Draft:
                if(newStatus == FestivalStatus.Published || 
                    newStatus == FestivalStatus.Cancelled)
                {
                    Status = newStatus;
                    return;
                }
                break;

            case FestivalStatus.Published:
                if(newStatus == FestivalStatus.OnSale ||
                    newStatus == FestivalStatus.Cancelled)
                {
                    Status = newStatus;
                    return;
                }
                break;

            case FestivalStatus.OnSale:
                if(newStatus == FestivalStatus.Live ||
                    newStatus == FestivalStatus.Cancelled)
                {
                    Status = newStatus;
                    return;
                }
                break;

            case FestivalStatus.Live:
                if(newStatus == FestivalStatus.Finished ||
                    newStatus == FestivalStatus.Cancelled)
                {
                    Status = newStatus;
                    return;
                }
                break;
        }

        throw new InvalidOperationException(
            $"Cannot change festival status from {Status} to {newStatus}.");
    }
};