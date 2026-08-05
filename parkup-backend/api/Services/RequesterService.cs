public class RequesterService : IRequesterService
{
    private readonly IRequesterRepository _requesterRepository;

    public RequesterService(IRequesterRepository requesterRepository)
    {
        _requesterRepository = requesterRepository;
    }

    public async Task<IReadOnlyList<RequesterDto>> GetAllRequestersAsync()
    {
        var requesters = await _requesterRepository.GetAllRequestersAsync();
        return requesters.Select(MapToDto).ToList();
    }

    public async Task<RequesterDto> CreateRequesterAsync(CreateRequesterDto request)
    {
        var name = request.Name.Trim();
        var email = request.Email.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Requester name is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Requester email is required.");
        }

        if (await _requesterRepository.EmailExistsAsync(email))
        {
            throw new RequesterAlreadyExistsException();
        }

        var requester = await _requesterRepository.CreateRequesterAsync(new Requester
        {
            Name = name,
            Email = email
        });

        return MapToDto(requester);
    }

    public async Task<RequesterEligibilityDto?> SetEligibilityAsync(
        int requesterId,
        SetRequesterEligibilityDto request)
    {
        if (requesterId <= 0 || request.EligibilityTypeId <= 0)
        {
            throw new ArgumentException("Requester and eligibility type IDs must be positive.");
        }

        if (!await _requesterRepository.RequesterExistsAsync(requesterId))
        {
            return null;
        }

        if (!await _requesterRepository.EligibilityTypeExistsAsync(request.EligibilityTypeId))
        {
            throw new EligibilityTypeNotFoundException();
        }

        var existingEligibility = await _requesterRepository.GetRequesterEligibilityAsync(
            requesterId,
            request.EligibilityTypeId);

        var eligibility = existingEligibility ?? await _requesterRepository.AddRequesterEligibilityAsync(
            new RequesterEligibility
            {
                RequesterId = requesterId,
                EligibilityTypeId = request.EligibilityTypeId
            });

        return new RequesterEligibilityDto
        {
            RequesterId = eligibility.RequesterId,
            EligibilityTypeId = eligibility.EligibilityTypeId
        };
    }

    public async Task<RequesterDto?> PatchRequesterAsync(int id, PatchRequesterDto request)
    {
        if (id <= 0)
        {
            return null;
        }

        var requester = await _requesterRepository.GetRequesterByIdAsync(id);
        if (requester == null)
        {
            return null;
        }

        if (request.Name != null)
        {
            var name = request.Name.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Requester name cannot be empty.");
            }

            requester.Name = name;
        }

        if (request.Email != null)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Requester email cannot be empty.");
            }

            if (await _requesterRepository.EmailExistsForAnotherRequesterAsync(email, id))
            {
                throw new RequesterAlreadyExistsException();
            }

            requester.Email = email;
        }

        await _requesterRepository.UpdateRequesterAsync(requester);
        return MapToDto(requester);
    }

    public async Task<bool> DeleteRequesterAsync(int id)
    {
        if (id <= 0)
        {
            return false;
        }

        if (await _requesterRepository.GetRequesterByIdAsync(id) == null)
        {
            return false;
        }

        if (await _requesterRepository.HasReservationsAsync(id))
        {
            throw new RequesterHasReservationsException();
        }

        return await _requesterRepository.DeleteRequesterAsync(id);
    }

    private static RequesterDto MapToDto(Requester requester)
    {
        return new RequesterDto
        {
            Id = requester.Id,
            Name = requester.Name,
            Email = requester.Email
        };
    }
}

public sealed class RequesterAlreadyExistsException : Exception
{
    public RequesterAlreadyExistsException()
        : base("A requester with this email already exists.")
    {
    }
}

public sealed class RequesterHasReservationsException : Exception
{
    public RequesterHasReservationsException()
        : base("The requester cannot be deleted because reservations belong to this requester.")
    {
    }
}

public sealed class EligibilityTypeNotFoundException : Exception
{
    public EligibilityTypeNotFoundException()
        : base("The eligibility type was not found.")
    {
    }
}
