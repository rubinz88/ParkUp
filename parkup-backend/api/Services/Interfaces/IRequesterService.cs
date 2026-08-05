public interface IRequesterService
{
    Task<IReadOnlyList<RequesterDto>> GetAllRequestersAsync();
    Task<RequesterDto> CreateRequesterAsync(CreateRequesterDto request);
    Task<RequesterEligibilityDto?> SetEligibilityAsync(int requesterId, SetRequesterEligibilityDto request);
    Task<RequesterDto?> PatchRequesterAsync(int id, PatchRequesterDto request);
    Task<bool> DeleteRequesterAsync(int id);
}
