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