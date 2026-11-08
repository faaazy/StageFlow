using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StageFlow.Api.Auth;
using StageFlow.Api.Contracts.Schedule;
using StageFlow.Application.Artists.Exceptions;
using StageFlow.Application.Festivals.Exceptions;
using StageFlow.Application.Schedule;
using StageFlow.Application.Schedule.Exceptions;
using StageFlow.Application.Stages.Exceptions;
using StageFlow.Domain.Festival;

namespace StageFlow.Api.Controllers;

[ApiController]
[Route("schedule/performances")]
public class PerformancesController(
    CreatePerformanceService createService, 
    GetPerformancesService getPerformancesService) : ControllerBase
{
    private readonly string _apiRoute = "/schedule/performances";

    private static SchedulePerformanceResponse ToScheduleResponse(Performance p) => new()
    {
            Id = p.Id,
            ArtistId = p.ArtistId,
            ArtistName = p.Artist.Name,
            StageId = p.StageId,
            StageName = p.Stage.Name,
            StartTime = p.StartTime,
            EndTime = p.EndTime
    };

    [HttpGet]
    public async Task<ActionResult<List<SchedulePerformanceResponse>>> GetAll()
    {
        var performances = await getPerformancesService.GetAllAsync();

        var response = performances.Select(ToScheduleResponse).ToList();

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PerformanceResponse>> Create(CreatePerformanceRequest performanceRequest)
    {
        if(!User.TryGetUserId(out var userId)) return Unauthorized();

        try
        {
            var performance = await createService.CreateAsync(
                userId,
                performanceRequest.StageId,
                performanceRequest.ArtistId,
                performanceRequest.StartTime,
                performanceRequest.EndTime
            );

             var response = new PerformanceResponse
            {
                Id = performance.Id,
                ArtistId = performance.ArtistId,
                StageId = performance.StageId,
                StartTime = performance.StartTime,
                EndTime = performance.EndTime,
            };

            return Created($"{_apiRoute}/{response.Id}", response);
        }
        catch (PerformanceScheduleConflictException)
        {
            return Conflict();
        }
        catch (StageNotFoundException)
        {
            return NotFound();
        }
        catch (FestivalNotFoundException)
        {
            return NotFound();
        }
        catch (FestivalAccessDeniedException)
        {
            return Forbid();
        }
        catch (PerformanceValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArtistNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("/festivals/{festivalId:guid}/performances")]
    [Authorize]
    public async Task<ActionResult<List<SchedulePerformanceResponse>>> GetByFestival(Guid festivalId)
    {
        try
        {
            var performances = await getPerformancesService.GetByFestivalAsync(festivalId);

            return Ok(performances.Select(ToScheduleResponse).ToList());
        }
        catch (FestivalNotFoundException)
        {
            return NotFound();
        }
    }
}