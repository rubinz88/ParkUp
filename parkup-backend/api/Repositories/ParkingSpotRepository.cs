using api.Context;
using Microsoft.EntityFrameworkCore;

public class ParkingSpotRepository : IParkingSpotRepository
{
    private readonly ParkUpDbContext _context;

    public ParkingSpotRepository(ParkUpDbContext context)
    {
        _context = context;
    }

    public async Task<ParkingSpot?> GetParkingSpotByIdAsync(int id)
    {
        return await _context.ParkingSpots.FindAsync(id);
    }

    public async Task<IEnumerable<ParkingSpot>> GetAllParkingSpotsAsync()
    {
        return await _context.ParkingSpots.AsNoTracking().ToListAsync();
    }

    public async Task AddParkingSpotAsync(ParkingSpot parkingSpot)
    {
        _context.ParkingSpots.Add(parkingSpot);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateParkingSpotAsync(ParkingSpot parkingSpot)
    {
        _context.Entry(parkingSpot).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteParkingSpotAsync(int id)
    {
        var parkingSpot = await _context.ParkingSpots.FindAsync(id);
        if (parkingSpot != null)
        {
            _context.ParkingSpots.Remove(parkingSpot);
            await _context.SaveChangesAsync();
        }
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}