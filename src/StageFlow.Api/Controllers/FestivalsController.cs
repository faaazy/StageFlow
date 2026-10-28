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
    GetFestivalService getFestivalService,
    UpdateFestivalService updateFestivalService) : ControllerBase
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
        if(!User.TryGetUserId(out var organizerId)) return Unauthorized();

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

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FestivalResponse>> Update(Guid id, UpdateFestivalRequest request)
    {
        if(!User.TryGetUserId(out var userId)) return Unauthorized();

        try
        {
            var updatedFestival = await updateFestivalService.UpdateAsync(
                id,
                userId,
                request.Name,
                request.Description,
                request.Location,
                request.StartDate,
                request.EndDate,
                request.TimeZone
            );

            return Ok(ToResponse(updatedFestival));
        }
        catch (FestivalNotFoundException notFoundEx)
        {
            return NotFound(notFoundEx.Message);
        }
        catch (FestivalAccessDeniedException)
        {
            return Forbid();
        }
        catch (FestivalValidationException validationEx)
        {
            return BadRequest(validationEx.Message);
        }
    }
}