using api.Context;
using Microsoft.EntityFrameworkCore;

public class RequesterRepository : IRequesterRepository
{
    private readonly ParkUpDbContext _context;

    public RequesterRepository(ParkUpDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Requester>> GetAllRequestersAsync()
    {
        return await _context.Requesters
            .AsNoTracking()
            .OrderBy(requester => requester.Id)
            .ToListAsync();
    }

    public Task<Requester?> GetRequesterByIdAsync(int id)
    {
        return _context.Requesters.FirstOrDefaultAsync(requester => requester.Id == id);
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        var normalizedEmail = email.ToLower();
        return _context.Requesters.AnyAsync(requester => requester.Email.ToLower() == normalizedEmail);
    }

    public Task<bool> EmailExistsForAnotherRequesterAsync(string email, int requesterId)
    {
        var normalizedEmail = email.ToLower();
        return _context.Requesters.AnyAsync(requester =>
            requester.Id != requesterId && requester.Email.ToLower() == normalizedEmail);
    }

    public async Task<Requester> CreateRequesterAsync(Requester requester)
    {
        _context.Requesters.Add(requester);
        await _context.SaveChangesAsync();
        return requester;
    }

    public async Task<Requester> UpdateRequesterAsync(Requester requester)
    {
        await _context.SaveChangesAsync();
        return requester;
    }

    public Task<bool> HasReservationsAsync(int requesterId)
    {
        return _context.ParkingReservations.AnyAsync(reservation => reservation.RequesterId == requesterId);
    }

    public async Task<bool> DeleteRequesterAsync(int id)
    {
        var requester = await _context.Requesters.FindAsync(id);
        if (requester == null)
        {
            return false;
        }

        _context.Requesters.Remove(requester);
        await _context.SaveChangesAsync();
        return true;
    }
}
