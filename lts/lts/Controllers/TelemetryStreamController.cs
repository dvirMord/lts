using Confluent.Kafka;
using lts.DTOs;
using lts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using static lts.Constants.Constatns.ControllerMessages;

[ApiController]
[Route("api/lts/v1")]
public class TelemetryStreamController : ControllerBase
{
    private readonly ITelemetryStreamService _telemetryStreamService;
    public TelemetryStreamController(ITelemetryStreamService telemetryStreamService)
    {
        this._telemetryStreamService = telemetryStreamService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartListening([FromBody] PartitionRequest request)
    {
        bool started = this._telemetryStreamService.StartStream(request.Partition);
        if (!started)
        {
            return Conflict(new { message = string.Format(StreamAlreadyRunning, request.Partition) });
        }

        return Ok(new { message = string.Format(StreamStarted, request.Partition) });
    }

    [HttpPost("stop")]
    public async Task<IActionResult> StopListening([FromBody] PartitionRequest request)
    {
        bool stopped = this._telemetryStreamService.StopStream(request.Partition);

        if (!stopped)
        {
            return NotFound(new { message = string.Format(StreamNotFound, request.Partition) });
        }

        return Ok(new { message = string.Format(StreamStopped, request.Partition) });
    }

    [HttpGet("status")]
    public IActionResult GetStatus([FromQuery] int? partition)
    {
        if (partition.HasValue)
        {
            bool isActive = this._telemetryStreamService.IsStreamActive(partition.Value);
            return Ok(new { partition = partition.Value, isActive });
        }

        var activePartitions = this._telemetryStreamService.GetActiveStreams();
        return Ok(new { activePartitions, count = activePartitions.Count });
    }
}
