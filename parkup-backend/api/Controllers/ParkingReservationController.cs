using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/reservations")]
public class ParkingReservationController : ControllerBase
{
    private readonly IParkingReservationService _reservationService;

    public ParkingReservationController(IParkingReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<ActionResult<ParkingReservationDto>> CreateReservation(
        CreateParkingReservationDto request)
    {
        try
        {
            var reservation = await _reservationService.CreateReservationAsync(request);
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (ReservationEligibilityException exception)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ParkingReservationDto>> GetReservation(int id)
    {
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        return reservation == null ? NotFound() : Ok(reservation);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> CancelReservation(int id)
    {
        var cancelled = await _reservationService.CancelReservationAsync(id);
        return cancelled ? NoContent() : NotFound();
    }
}
