public class ParkingSpotService : IParkingSpotService
{
    private readonly IParkingSpotRepository _parkingSpotRepository;

    public ParkingSpotService(IParkingSpotRepository parkingSpotRepository)
    {
        _parkingSpotRepository = parkingSpotRepository;
    }

    public async Task<ParkingSpotDto?> GetParkingSpotByIdAsync(int id)
    {
        var parkingSpot = await _parkingSpotRepository.GetParkingSpotByIdAsync(id);
        return parkingSpot == null ? null : MapToDto(parkingSpot);
    }

    public async Task<IEnumerable<ParkingSpotDto>> GetAllParkingSpotsAsync()
    {
        var parkingSpots = await _parkingSpotRepository.GetAllParkingSpotsAsync();
        return parkingSpots.Select(MapToDto);
    }

    public async Task AddParkingSpotAsync(ParkingSpotDto parkingSpotDto)
    {
        var parkingSpot = MapToEntity(parkingSpotDto);
        await _parkingSpotRepository.AddParkingSpotAsync(parkingSpot);
    }

    public async Task UpdateParkingSpotAsync(ParkingSpotDto parkingSpotDto)
    {
        var parkingSpot = MapToEntity(parkingSpotDto);
        await _parkingSpotRepository.UpdateParkingSpotAsync(parkingSpot);
    }

    public async Task DeleteParkingSpotAsync(int id)
    {
        await _parkingSpotRepository.DeleteParkingSpotAsync(id);
    }
    public async Task PatchParkingSpotAsync(int id, ParkingSpotDto parkingSpotDto)
    {
        var existingParkingSpot = await _parkingSpotRepository.GetParkingSpotByIdAsync(id);
        if (existingParkingSpot == null)
        {
            throw new Exception($"Parking spot with ID {id} not found.");
        }

        // Update only the properties that are not null in the DTO
        if (parkingSpotDto.ParkingSpotName != null)
            existingParkingSpot.ParkingSpotName = parkingSpotDto.ParkingSpotName;

        if (parkingSpotDto.BuildingId != 0)
            existingParkingSpot.BuildingId = parkingSpotDto.BuildingId;

        if (parkingSpotDto.FloorNumber != 0)
            existingParkingSpot.FloorNumber = parkingSpotDto.FloorNumber;

        if (parkingSpotDto.Price != 0)
            existingParkingSpot.Price = parkingSpotDto.Price;

        if (parkingSpotDto.EligibilityTypeId != 0)
            existingParkingSpot.EligibilityTypeId = parkingSpotDto.EligibilityTypeId;

        await _parkingSpotRepository.UpdateParkingSpotAsync(existingParkingSpot);
    }

    private ParkingSpotDto MapToDto(ParkingSpot parkingSpot)
    {
        return new ParkingSpotDto
        {
            Id = parkingSpot.Id,
            ParkingSpotName = parkingSpot.ParkingSpotName,
            BuildingId = parkingSpot.BuildingId,
            FloorNumber = parkingSpot.FloorNumber,
            Price = parkingSpot.Price,
            EligibilityTypeId = parkingSpot.EligibilityTypeId
        };
    }

    private ParkingSpot MapToEntity(ParkingSpotDto parkingSpotDto)
    {
        return new ParkingSpot
        {
            Id = parkingSpotDto.Id,
            ParkingSpotName = parkingSpotDto.ParkingSpotName,
            BuildingId = parkingSpotDto.BuildingId,
            FloorNumber = parkingSpotDto.FloorNumber,
            Price = parkingSpotDto.Price,
            EligibilityTypeId = parkingSpotDto.EligibilityTypeId
        };
    }
}
