using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StageFlow.Application.Festivals;
using StageFlow.Application.Festivals.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("festivals")]
public class FestivalsController(
    CreateFestivalService createFestivalService,
    GetFestivalService getFestivalService) : ControllerBase
{
    private static FestivalResponse ToResponse(Festival festival) => new()
    {
        Id = festival.Id,
        OrganizerId = festival.OrganizerId,
        Name = festival.Name,
        Description = festival.Description,
        Location = festival.Location,
        StartDate = festival.StartDate,
        EndDate = festival.EndDate,
        Status = festival.Status,
        TimeZone = festival.TimeZone
    };

    [HttpPost]
    public async Task<ActionResult<FestivalResponse>> Create(CreateFestivalRequest request)
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if(!Guid.TryParse(sub, out var organizerId)) 
            return Unauthorized();

        try
        {
            var festival = await createFestivalService.CreateAsync(
                organizerId,
                request.Name,
                request.Description,
                request.Location,
                request.StartDate,
                request.EndDate,
                request.TimeZone
            );

            var response = ToResponse(festival);

            return Created($"/festivals/{response.Id}", response);
        }
        catch(FestivalValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FestivalResponse>> Get(Guid id)
    {
        var festival = await getFestivalService.GetByIdAsync(id);

        return festival is null ? NotFound() : Ok(ToResponse(festival));


    }
}