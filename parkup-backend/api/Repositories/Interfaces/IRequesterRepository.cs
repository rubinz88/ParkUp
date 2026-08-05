public interface IRequesterRepository
{
    Task<IReadOnlyList<Requester>> GetAllRequestersAsync();
    Task<Requester?> GetRequesterByIdAsync(int id);
    Task<bool> RequesterExistsAsync(int id);
    Task<bool> EligibilityTypeExistsAsync(int id);
    Task<RequesterEligibility?> GetRequesterEligibilityAsync(int requesterId, int eligibilityTypeId);
    Task<RequesterEligibility> AddRequesterEligibilityAsync(RequesterEligibility eligibility);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> EmailExistsForAnotherRequesterAsync(string email, int requesterId);
    Task<Requester> CreateRequesterAsync(Requester requester);
    Task<Requester> UpdateRequesterAsync(Requester requester);
    Task<bool> HasReservationsAsync(int requesterId);
    Task<bool> DeleteRequesterAsync(int id);
}
