using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/parkingspots/{parkingSpotId:int}/reservations")]
public class ParkingSpotReservationsController : ControllerBase
{
    private readonly IParkingReservationService _reservationService;

    public ParkingSpotReservationsController(IParkingReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ParkingReservationDto>>> GetReservations(int parkingSpotId)
    {
        try
        {
            var reservations = await _reservationService.GetReservationsByParkingSpotIdAsync(parkingSpotId);
            return Ok(reservations);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }
}
