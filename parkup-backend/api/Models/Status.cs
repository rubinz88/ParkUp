public partial class Status
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<ParkingReservation> ParkingReservations { get; set; } = new HashSet<ParkingReservation>();
}
