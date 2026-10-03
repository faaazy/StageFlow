using Microsoft.AspNetCore.Mvc;
using StageFlow.Application.Schedule;
using StageFlow.Application.Schedule.Exceptions;

namespace StageFlow.Api.Controllers;

[ApiController]
[Route("schedule/performances")]
public class PerformancesController(CreatePerformanceService createService, GetPerformancesService getPerformancesService) : ControllerBase
{
    private readonly string _apiRoute = "/schedule/performances";

    [HttpGet]
    public async Task<ActionResult<List<SchedulePerformanceResponse>>> GetAll()
    {
        var performances = await getPerformancesService.GetAllAsync();

        var response = performances.Select(p => new SchedulePerformanceResponse
        {
            Id = p.Id,
            ArtistId = p.ArtistId,
            ArtistName = p.Artist.Name,
            StageId = p.StageId,
            StageName = p.Stage.Name,
            StartTime = p.StartTime,
            EndTime = p.EndTime
        }).ToList();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<PerformanceResponse>> Create(CreatePerformanceRequest performanceRequest)
    {
        try
        {
            var performance = await createService.CreateAsync(
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
    }
}