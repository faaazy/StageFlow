using StageFlow.Application.Festivals.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Application.Festivals;

public class CreateFestivalService(IFestivalRepository festivalRepository)
{
    public async Task<Festival> CreateAsync(
        Guid organizerId,
        string name, 
        string? description,
        string location,
        DateTime startDate,
        DateTime endDate,
        string timeZone
    )
    {
        if(string.IsNullOrWhiteSpace(name)) 
            throw new FestivalValidationException("Name is required"); 

        if(startDate.Kind != DateTimeKind.Utc || endDate.Kind != DateTimeKind.Utc)
            throw new FestivalValidationException("Dates must be in UTC, e.g. 2026-11-01T10:00:00Z"); 

        if(endDate <= startDate) 
            throw new FestivalValidationException("End date must be later than start date"); 

        var festival = new Festival
        {
            OrganizerId = organizerId,
            Name = name.Trim(),
            Description = description,
            Location = location,
            StartDate = startDate,
            EndDate = endDate,
            TimeZone = timeZone
        };

        return await festivalRepository.AddAsync(festival);
    }
}