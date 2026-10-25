using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StageFlow.Application.Festivals;
using StageFlow.Application.Festivals.Exceptions;

namespace StageFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("festivals")]
public class FestivalsController(CreateFestivalService createService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<FestivalResponse>> Create(CreateFestivalRequest request)
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if(!Guid.TryParse(sub, out var organizerId)) 
            return Unauthorized();

        try
        {
            var festival = await createService.CreateAsync(
                organizerId,
                request.Name,
                request.Description,
                request.Location,
                request.StartDate,
                request.EndDate,
                request.TimeZone
            );

            var response = new FestivalResponse
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

            return Created($"/festivals/{response.Id}", response);
        }
        catch(FestivalValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}