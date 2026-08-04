public class ParkingSpotDto
{
    public int Id { get; set; }
    public string ParkingSpotName { get; set; } = null!;
    public int BuildingId { get; set; }
    public int FloorNumber { get; set; }
    public decimal Price { get; set; }
    public int? EligibilityTypeId { get; set; }
}
