public partial class ParkingSpot
{
    public int Id { get; set; }
    public required string ParkingSpotName { get; set; }
    public int BuildingId { get; set; }
    public int FloorNumber { get; set; }
    public decimal Price { get; set; }
    public int EligibilityTypeId { get; set; }

    public Building Building { get; set; } = null!;
    public EligibilityType? EligibilityType { get; set; }
    public virtual ICollection<ParkingReservation> ParkingReservations { get; set; } = new HashSet<ParkingReservation>();
}
