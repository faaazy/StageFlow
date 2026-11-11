using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StageFlow.Api.Auth;
using StageFlow.Api.Contracts.Artists;
using StageFlow.Application.Artists;
using StageFlow.Application.Artists.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("artists")]
public class ArtistsController(
    CreateArtistService createArtistService,
    GetArtistsService getArtistsService) : ControllerBase
{
    private static ArtistResponse ToResponse(Artist artist) => new()
    {
        Id = artist.Id,
        Name = artist.Name,
        OrganizerId = artist.OrganizerId
    };

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

    [HttpGet("mine")]
    public async Task<ActionResult<List<ArtistResponse>>> GetMine()
    {
        if(!User.TryGetUserId(out var userId)) return Unauthorized();

        var artists = await getArtistsService.GetByOrganizerAsync(userId);

        return Ok(artists.Select(ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ArtistResponse>> GetById(Guid id)
    {
        if(!User.TryGetUserId(out var userId)) return Unauthorized();

        try
        {
            var artist = await getArtistsService.GetByIdAsync(id, userId);

            return Ok(ToResponse(artist));
        }
        catch (ArtistNotFoundException)
        {
            return NotFound();
        }
    }
}