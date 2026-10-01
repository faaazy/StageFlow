using Microsoft.AspNetCore.Mvc;
using StageFlow.Application.Schedule;
using StageFlow.Application.Schedule.Exceptions;

namespace StageFlow.Api.Controllers;

[ApiController]
[Route("schedule/performances")]
public class PerformancesController(CreatePerformanceService performanceService) : ControllerBase
{
    private readonly string _apiRoute = "/schedule/performances";
    [HttpPost]
    public async Task<ActionResult<PerformanceResponse>> Create(CreatePerformanceRequest performanceRequest)
    {
        try
        {
            var performance = await performanceService.CreateAsync(
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