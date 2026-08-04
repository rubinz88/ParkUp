public partial class EligibilityType
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    
    public virtual ICollection<ParkingSpot> ParkingSpots { get; set; } = new HashSet<ParkingSpot>();
    public virtual ICollection<RequesterEligibility> RequesterEligibilities { get; set; } = new HashSet<RequesterEligibility>();
}
