public partial class Building
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public int NumberOfFloors { get; set; }
    public int NumberOfSpots { get; set; }

    public virtual ICollection<ParkingSpot> ParkingSpots { get; set; } = new HashSet<ParkingSpot>();
}
