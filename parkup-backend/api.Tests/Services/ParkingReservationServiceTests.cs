public class ParkingReservationServiceTests
{
    [Fact]
    public async Task CreateReservationAsync_CreatesPendingUtcReservation()
    {
        var repository = new FakeParkingReservationsRepository();
        var service = new ParkingReservationService(repository);
        var startingDate = new DateTime(2026, 8, 10, 10, 0, 0, DateTimeKind.Unspecified);
        var endingDate = new DateTime(2026, 8, 10, 12, 0, 0, DateTimeKind.Unspecified);

        var result = await service.CreateReservationAsync(new CreateParkingReservationDto
        {
            ParkingSpotId = 1,
            RequesterId = 10,
            StartingDate = startingDate,
            EndingDate = endingDate
        });

        Assert.Equal(1, result.Id);
        Assert.Equal(1, result.StatusId);
        Assert.Equal(DateTimeKind.Utc, result.StartingDate.Kind);
        Assert.Equal(DateTimeKind.Utc, result.EndingDate.Kind);
        Assert.Single(repository.Reservations);
    }

    [Fact]
    public async Task CreateReservationAsync_WhenDatesOverlap_ThrowsConflict()
    {
        var repository = new FakeParkingReservationsRepository
        {
            Reservations =
            {
                new ParkingReservation
                {
                    Id = 1,
                    ParkingSpotId = 1,
                    RequesterId = 20,
                    StatusId = 1,
                    StartingDate = new DateTime(2026, 8, 10, 10, 0, 0, DateTimeKind.Utc),
                    EndingDate = new DateTime(2026, 8, 10, 12, 0, 0, DateTimeKind.Utc)
                }
            }
        };
        var service = new ParkingReservationService(repository);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateReservationAsync(new CreateParkingReservationDto
            {
                ParkingSpotId = 1,
                RequesterId = 10,
                StartingDate = new DateTime(2026, 8, 10, 11, 0, 0),
                EndingDate = new DateTime(2026, 8, 10, 13, 0, 0)
            }));

        Assert.Contains("already reserved", exception.Message);
    }

    [Fact]
    public async Task CreateReservationAsync_WhenStartingDateIsNotBeforeEndingDate_ThrowsValidationError()
    {
        var service = new ParkingReservationService(new FakeParkingReservationsRepository());

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateReservationAsync(new CreateParkingReservationDto
            {
                ParkingSpotId = 1,
                RequesterId = 10,
                StartingDate = new DateTime(2026, 8, 10, 12, 0, 0),
                EndingDate = new DateTime(2026, 8, 10, 10, 0, 0)
            }));
    }

    [Fact]
    public async Task CreateReservationAsync_WhenParkingSpotDoesNotExist_ThrowsNotFoundError()
    {
        var repository = new FakeParkingReservationsRepository
        {
            ParkingSpotExists = false
        };
        var service = new ParkingReservationService(repository);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.CreateReservationAsync(new CreateParkingReservationDto
            {
                ParkingSpotId = 999,
                RequesterId = 10,
                StartingDate = new DateTime(2026, 8, 10, 10, 0, 0),
                EndingDate = new DateTime(2026, 8, 10, 12, 0, 0)
            }));
    }

    [Fact]
    public async Task CreateReservationAsync_WhenRequesterIsNotEligible_ThrowsEligibilityError()
    {
        var repository = new FakeParkingReservationsRepository
        {
            RequesterEligible = false
        };
        var service = new ParkingReservationService(repository);

        await Assert.ThrowsAsync<ReservationEligibilityException>(() =>
            service.CreateReservationAsync(new CreateParkingReservationDto
            {
                ParkingSpotId = 1,
                RequesterId = 10,
                StartingDate = new DateTime(2026, 8, 10, 10, 0, 0),
                EndingDate = new DateTime(2026, 8, 10, 12, 0, 0)
            }));
    }

    [Fact]
    public async Task CancelReservationAsync_SetsCancelledStatus()
    {
        var repository = new FakeParkingReservationsRepository
        {
            Reservations =
            {
                new ParkingReservation
                {
                    Id = 1,
                    ParkingSpotId = 1,
                    RequesterId = 10,
                    StatusId = 1
                }
            }
        };
        var service = new ParkingReservationService(repository);

        var result = await service.CancelReservationAsync(1);

        Assert.True(result);
        Assert.Equal(4, repository.Reservations.Single().StatusId);
    }

    private sealed class FakeParkingReservationsRepository : IParkingReservationsRepository
    {
        public List<ParkingReservation> Reservations { get; set; } = new();
        public bool ParkingSpotExists { get; set; } = true;
        public bool RequesterExists { get; set; } = true;
        public bool RequesterEligible { get; set; } = true;

        public Task<ParkingReservation?> GetReservationByIdAsync(int id)
        {
            return Task.FromResult(Reservations.SingleOrDefault(reservation => reservation.Id == id));
        }

        public Task<IReadOnlyList<ParkingReservation>> ListReservationsByParkingSpotIdAsync(int parkingSpotId)
        {
            IReadOnlyList<ParkingReservation> result = Reservations
                .Where(reservation => reservation.ParkingSpotId == parkingSpotId)
                .OrderBy(reservation => reservation.StartingDate)
                .ToList();

            return Task.FromResult(result);
        }

        public Task<bool> ParkingSpotExistsAsync(int parkingSpotId)
        {
            return Task.FromResult(ParkingSpotExists);
        }

        public Task<bool> RequesterExistsAsync(int requesterId)
        {
            return Task.FromResult(RequesterExists);
        }

        public Task<bool> IsRequesterEligibleForParkingSpotAsync(int requesterId, int parkingSpotId)
        {
            return Task.FromResult(RequesterEligible);
        }

        public Task<bool> HasActiveReservationAsync(
            int parkingSpotId,
            DateTime startingDate,
            DateTime endingDate)
        {
            var result = Reservations.Any(reservation =>
                reservation.ParkingSpotId == parkingSpotId &&
                reservation.StatusId != 3 &&
                reservation.StatusId != 4 &&
                reservation.StartingDate < endingDate &&
                reservation.EndingDate > startingDate);

            return Task.FromResult(result);
        }

        public Task<ParkingReservation> CreateReservationAsync(ParkingReservation reservation)
        {
            reservation.Id = Reservations.Count == 0 ? 1 : Reservations.Max(item => item.Id) + 1;
            Reservations.Add(reservation);
            return Task.FromResult(reservation);
        }

        public Task<bool> CancelReservationAsync(int id)
        {
            var reservation = Reservations.SingleOrDefault(item => item.Id == id);
            if (reservation == null)
            {
                return Task.FromResult(false);
            }

            reservation.StatusId = 4;
            return Task.FromResult(true);
        }
    }
}
