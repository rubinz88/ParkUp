public interface IRequesterRepository
{
    Task<IReadOnlyList<Requester>> GetAllRequestersAsync();
    Task<Requester?> GetRequesterByIdAsync(int id);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> EmailExistsForAnotherRequesterAsync(string email, int requesterId);
    Task<Requester> CreateRequesterAsync(Requester requester);
    Task<Requester> UpdateRequesterAsync(Requester requester);
    Task<bool> HasReservationsAsync(int requesterId);
    Task<bool> DeleteRequesterAsync(int id);
}
