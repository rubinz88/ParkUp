public interface IParkingReservationsRepository
{
    Task<ParkingReservation?> GetReservationByIdAsync(int id);
    Task<IReadOnlyList<ParkingReservation>> ListReservationsByParkingSpotIdAsync(int parkingSpotId);
    Task<bool> ParkingSpotExistsAsync(int parkingSpotId);
    Task<bool> RequesterExistsAsync(int requesterId);
    Task<bool> HasActiveReservationAsync(int parkingSpotId, DateTime startingDate, DateTime endingDate);
    Task<ParkingReservation> CreateReservationAsync(ParkingReservation reservation);
    Task<bool> CancelReservationAsync(int id);
}
