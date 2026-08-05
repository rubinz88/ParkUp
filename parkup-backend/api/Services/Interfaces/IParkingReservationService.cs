public interface IParkingReservationService
{
    Task<ParkingReservationDto> CreateReservationAsync(CreateParkingReservationDto request);
    Task<ParkingReservationDto?> GetReservationByIdAsync(int id);
    Task<IReadOnlyList<ParkingReservationDto>> GetReservationsByParkingSpotIdAsync(int parkingSpotId);
    Task<bool> CancelReservationAsync(int id);
}
