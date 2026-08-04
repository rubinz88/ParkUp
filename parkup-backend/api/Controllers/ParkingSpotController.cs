using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ParkingSpotController : ControllerBase
{
    private readonly IParkingSpotService _parkingSpotService;

    public ParkingSpotController(IParkingSpotService parkingSpotService)
    {
        _parkingSpotService = parkingSpotService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParkingSpotDto?>> GetParkingSpotById(int id)
    {
        var parkingSpot = await _parkingSpotService.GetParkingSpotByIdAsync(id);
        if (parkingSpot == null)
        {
            return NotFound();
        }
        return Ok(parkingSpot);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParkingSpotDto>>> GetAllParkingSpots()
    {
        var parkingSpots = await _parkingSpotService.GetAllParkingSpotsAsync();
        return Ok(parkingSpots);
    }

    [HttpPost]
    public async Task<IActionResult> AddParkingSpot(ParkingSpotDto parkingSpot)
    {
        await _parkingSpotService.AddParkingSpotAsync(parkingSpot);
        return CreatedAtAction(nameof(GetParkingSpotById), new { id = parkingSpot.Id }, parkingSpot);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateParkingSpot(int id, ParkingSpotDto parkingSpot)
    {
        if (id != parkingSpot.Id)
        {
            return BadRequest();
        }

        await _parkingSpotService.UpdateParkingSpotAsync(parkingSpot);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParkingSpot(int id)
    {
        await _parkingSpotService.DeleteParkingSpotAsync(id);
        return NoContent();
    }    
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchParkingSpot(int id, ParkingSpotDto parkingSpot)
    {
        await _parkingSpotService.PatchParkingSpotAsync(id, parkingSpot);
        return NoContent();
    }
}
