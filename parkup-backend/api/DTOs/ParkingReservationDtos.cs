public sealed class CreateParkingReservationDto
{
    public int ParkingSpotId { get; set; }
    public int RequesterId { get; set; }
    public DateTime StartingDate { get; set; }
    public DateTime EndingDate { get; set; }
}

public sealed class ParkingReservationDto
{
    public int Id { get; set; }
    public int ParkingSpotId { get; set; }
    public int RequesterId { get; set; }
    public int StatusId { get; set; }
    public DateTime StartingDate { get; set; }
    public DateTime EndingDate { get; set; }
}
