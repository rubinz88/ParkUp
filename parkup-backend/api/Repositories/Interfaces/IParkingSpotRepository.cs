public interface IParkingSpotRepository
{
    Task<ParkingSpot?> GetParkingSpotByIdAsync(int id);
    Task<IEnumerable<ParkingSpot>> GetAllParkingSpotsAsync();
    Task AddParkingSpotAsync(ParkingSpot parkingSpot);
    Task UpdateParkingSpotAsync(ParkingSpot parkingSpot);
    Task DeleteParkingSpotAsync(int id);
    Task SaveChangesAsync();
}
