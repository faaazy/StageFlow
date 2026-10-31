using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StageFlow.Api.Auth;
using StageFlow.Api.Contracts.Stages;
using StageFlow.Application.Festivals.Exceptions;
using StageFlow.Application.Stages;
using StageFlow.Application.Stages.Exceptions;

namespace StageFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("festivals/{festivalId:guid}/stages")]
public class StagesController(CreateStageService createStageService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<StageResponse>> Create(Guid festivalId, CreateStageRequest request)
    {
        if(!User.TryGetUserId(out var userId)) return Unauthorized();

        try
        {
            var stage = await createStageService.CreateAsync(festivalId, userId, request.Name);

            var response = new StageResponse
            {
                Id = stage.Id,
                FestivalId = stage.FestivalId,
                Name = stage.Name
            };

            return Created($"/festivals/{festivalId}/stages/{stage.Id}", response);
        }
        catch (FestivalNotFoundException notFoundEx)
        {
            return NotFound(notFoundEx.Message);
        }
        catch (FestivalAccessDeniedException)
        {
            return Forbid();
        }
        catch (StageValidationException stageValidationEx)
        {
            return BadRequest(stageValidationEx.Message);
        }
    }
}