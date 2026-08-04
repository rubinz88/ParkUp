public interface IParkingSpotService 
{
    Task<ParkingSpotDto?> GetParkingSpotByIdAsync(int id);
    Task<IEnumerable<ParkingSpotDto>> GetAllParkingSpotsAsync();
    Task AddParkingSpotAsync(ParkingSpotDto parkingSpot);
    Task UpdateParkingSpotAsync(ParkingSpotDto parkingSpot);
    Task PatchParkingSpotAsync(int id, ParkingSpotDto parkingSpot);
    Task DeleteParkingSpotAsync(int id);
}
