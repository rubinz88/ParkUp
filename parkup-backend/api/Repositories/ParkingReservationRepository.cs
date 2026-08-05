using api.Context;
using Microsoft.EntityFrameworkCore;

public class ParkingReservationRepository : IParkingReservationsRepository
{
    private readonly ParkUpDbContext _context;

    public ParkingReservationRepository(ParkUpDbContext context)
    {
        _context = context;
    }

    public async Task<ParkingReservation?> GetReservationByIdAsync(int id)
    {
        return await _context.ParkingReservations
            .AsNoTracking()
            .FirstOrDefaultAsync(reservation => reservation.Id == id);
    }

    public async Task<IReadOnlyList<ParkingReservation>> ListReservationsByParkingSpotIdAsync(int parkingSpotId)
    {
        return await _context.ParkingReservations
            .AsNoTracking()
            .Where(reservation => reservation.ParkingSpotId == parkingSpotId)
            .OrderBy(reservation => reservation.StartingDate)
            .ToListAsync();
    }

    public async Task<bool> ParkingSpotExistsAsync(int parkingSpotId)
    {
        return await _context.ParkingSpots.AnyAsync(spot => spot.Id == parkingSpotId);
    }

    public async Task<bool> RequesterExistsAsync(int requesterId)
    {
        return await _context.Requesters.AnyAsync(requester => requester.Id == requesterId);
    }

    public async Task<bool> HasActiveReservationAsync(
        int parkingSpotId,
        DateTime startingDate,
        DateTime endingDate)
    {
        return await _context.ParkingReservations.AnyAsync(reservation =>
            reservation.ParkingSpotId == parkingSpotId &&
            reservation.StatusId != 3 &&
            reservation.StatusId != 4 &&
            reservation.StartingDate < endingDate &&
            reservation.EndingDate > startingDate);
    }

    public async Task<ParkingReservation> CreateReservationAsync(ParkingReservation reservation)
    {
        _context.ParkingReservations.Add(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task<bool> CancelReservationAsync(int id)
    {
        var reservation = await _context.ParkingReservations.FindAsync(id);
        if (reservation == null)
        {
            return false;
        }

        reservation.StatusId = 4;
        await _context.SaveChangesAsync();
        return true;
    }
}
