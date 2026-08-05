using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/requesters")]
public class RequesterController : ControllerBase
{
    private readonly IRequesterService _requesterService;

    public RequesterController(IRequesterService requesterService)
    {
        _requesterService = requesterService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RequesterDto>>> GetAllRequesters()
    {
        return Ok(await _requesterService.GetAllRequestersAsync());
    }

    [HttpPost]
    public async Task<ActionResult<RequesterDto>> CreateRequester(CreateRequesterDto request)
    {
        try
        {
            var requester = await _requesterService.CreateRequesterAsync(request);
            return Created("/api/requesters", requester);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (RequesterAlreadyExistsException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<RequesterDto>> PatchRequester(int id, PatchRequesterDto request)
    {
        try
        {
            var requester = await _requesterService.PatchRequesterAsync(id, request);
            return requester == null ? NotFound() : Ok(requester);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (RequesterAlreadyExistsException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRequester(int id)
    {
        try
        {
            var deleted = await _requesterService.DeleteRequesterAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (RequesterHasReservationsException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }
}
