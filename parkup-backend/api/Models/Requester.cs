public partial class Requester
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    public required string Email { get; set; }

    public virtual ICollection<RequesterEligibility> RequesterEligibilities { get; set; } = new HashSet<RequesterEligibility>();
    public virtual ICollection<ParkingReservation> ParkingReservations { get; set; } = new HashSet<ParkingReservation>();
}
