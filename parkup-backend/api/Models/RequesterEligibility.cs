public partial class RequesterEligibility
{
    public int Id { get; set; }
    public int RequesterId { get; set; }
    public int EligibilityTypeId { get; set; }

    public virtual Requester Requester { get; set; } = null!;
    public virtual EligibilityType EligibilityType { get; set; } = null!;
}
