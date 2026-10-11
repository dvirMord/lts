using lts.DTOs;
using static lts.Constants.Constatns.ControllerMessages;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/lts/v1")]
public class ConsumerController : ControllerBase
{
    public ConsumerController()
    {
        
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartListening([FromBody] PartitionRequest request, CancellationToken cancellationToken)
    {
        return Ok(new { message = string.Format(StartListeningMessage, request.Partition), partition = request.Partition });
    }

    [HttpPost("stop")]
    public async Task<IActionResult> StopListening([FromBody] PartitionRequest request, CancellationToken cancellationToken)
    {
        return Ok(new { message = string.Format(StopListeningMessage, request.Partition), partition = request.Partition });
    }
}