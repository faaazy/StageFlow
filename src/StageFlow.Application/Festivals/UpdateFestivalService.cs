using StageFlow.Application.Festivals.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Application.Festivals;

public class UpdateFestivalService(IFestivalRepository festivalRepository)
{
    public async Task<Festival> UpdateAsync(
        Guid festivalId, 
        Guid userId,
        string name,
        string? description,
        string location,
        DateTime startDate,
        DateTime endDate,
        string timeZone)
    {
        var festival = await festivalRepository.GetByIdAsync(festivalId) 
                ?? throw new FestivalNotFoundException();

        if (festival.OrganizerId != userId) throw new FestivalAccessDeniedException();

        if(string.IsNullOrWhiteSpace(name)) 
            throw new FestivalValidationException("Name is required"); 

        if(string.IsNullOrWhiteSpace(location)) 
            throw new FestivalValidationException("Location is required"); 

        if(startDate.Kind != DateTimeKind.Utc || endDate.Kind != DateTimeKind.Utc)
            throw new FestivalValidationException("Dates must be in UTC, e.g. 2026-11-01T10:00:00Z"); 

        if(endDate <= startDate) 
            throw new FestivalValidationException("End date must be later than start date"); 

        festival.Name = name.Trim();
        festival.Description = description?.Trim();
        festival.Location = location.Trim();
        festival.StartDate = startDate;
        festival.EndDate = endDate;
        festival.TimeZone = timeZone;

        await festivalRepository.UpdateAsync(festival);

        return festival;
    }
}