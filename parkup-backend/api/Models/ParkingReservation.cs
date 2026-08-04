public partial class ParkingReservation
{
    public int Id { get; set; }
    public int ParkingSpotId { get; set; }
    public int RequesterId { get; set; }
    public DateTime StartingDate { get; set; }
    public DateTime EndingDate { get; set; }

    public virtual ParkingSpot ParkingSpot { get; set; } = null!;
    public virtual Requester Requester { get; set; } = null!;
    public virtual Status Status { get; set; } = null!;
}
