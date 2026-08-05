public class ParkingReservationService : IParkingReservationService
{
    private const int PendingStatusId = 1;
    private readonly IParkingReservationsRepository _reservationRepository;

    public ParkingReservationService(IParkingReservationsRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<ParkingReservationDto> CreateReservationAsync(CreateParkingReservationDto request)
    {
        if (request.ParkingSpotId <= 0 || request.RequesterId <= 0)
        {
            throw new ArgumentException("Parking spot and requester IDs must be positive.");
        }

        if (request.StartingDate == default || request.EndingDate == default)
        {
            throw new ArgumentException("Starting and ending dates are required.");
        }

        var startingDate = NormalizeToUtc(request.StartingDate);
        var endingDate = NormalizeToUtc(request.EndingDate);

        if (startingDate >= endingDate)
        {
            throw new ArgumentException("Starting date must be earlier than ending date.");
        }

        if (!await _reservationRepository.ParkingSpotExistsAsync(request.ParkingSpotId))
        {
            throw new KeyNotFoundException("Parking spot was not found.");
        }

        if (!await _reservationRepository.RequesterExistsAsync(request.RequesterId))
        {
            throw new KeyNotFoundException("Requester was not found.");
        }

        if (!await _reservationRepository.IsRequesterEligibleForParkingSpotAsync(
                request.RequesterId,
                request.ParkingSpotId))
        {
            throw new ReservationEligibilityException();
        }

        if (await _reservationRepository.HasActiveReservationAsync(
                request.ParkingSpotId,
                startingDate,
                endingDate))
        {
            throw new InvalidOperationException("The parking spot is already reserved for the requested period.");
        }

        var reservation = await _reservationRepository.CreateReservationAsync(new ParkingReservation
        {
            ParkingSpotId = request.ParkingSpotId,
            RequesterId = request.RequesterId,
            StatusId = PendingStatusId,
            StartingDate = startingDate,
            EndingDate = endingDate
        });

        return MapToDto(reservation);
    }

    public async Task<ParkingReservationDto?> GetReservationByIdAsync(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        var reservation = await _reservationRepository.GetReservationByIdAsync(id);
        return reservation == null ? null : MapToDto(reservation);
    }

    public async Task<IReadOnlyList<ParkingReservationDto>> GetReservationsByParkingSpotIdAsync(int parkingSpotId)
    {
        if (parkingSpotId <= 0)
        {
            throw new ArgumentException("Parking spot ID must be positive.");
        }

        var reservations = await _reservationRepository.ListReservationsByParkingSpotIdAsync(parkingSpotId);
        return reservations.Select(MapToDto).ToList();
    }

    public Task<bool> CancelReservationAsync(int id)
    {
        if (id <= 0)
        {
            return Task.FromResult(false);
        }

        return _reservationRepository.CancelReservationAsync(id);
    }

    private static DateTime NormalizeToUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }

    private static ParkingReservationDto MapToDto(ParkingReservation reservation)
    {
        return new ParkingReservationDto
        {
            Id = reservation.Id,
            ParkingSpotId = reservation.ParkingSpotId,
            RequesterId = reservation.RequesterId,
            StatusId = reservation.StatusId,
            StartingDate = reservation.StartingDate,
            EndingDate = reservation.EndingDate
        };
    }
}

public sealed class ReservationEligibilityException : Exception
{
    public ReservationEligibilityException()
        : base("The requester is not eligible to reserve this parking spot.")
    {
    }
}
