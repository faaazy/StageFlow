using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StageFlow.Application.Artists;
using StageFlow.Application.Artists.Exceptions;

namespace StageFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("artists")]
public class ArtistsController(CreateArtistService createArtistService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ArtistResponse>> Create(CreateArtistRequest request)
    {
        if(!User.TryGetUserId(out var organizerId)) return Unauthorized();

        try
        {
            var artist = await createArtistService.CreateAsync(organizerId, request.Name);

            var response = new ArtistResponse
            {
                Id = artist.Id,
                OrganizerId = artist.OrganizerId,
                Name = artist.Name
            };

            return Created($"/artists/{artist.Id}", response);
        }
        catch (ArtistValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}