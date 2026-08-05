using System.ComponentModel.DataAnnotations;

public sealed class CreateRequesterDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public sealed class RequesterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public sealed class PatchRequesterDto
{
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
}

public sealed class SetRequesterEligibilityDto
{
    [Range(1, 3)]
    public int EligibilityTypeId { get; set; }
}

public sealed class RequesterEligibilityDto
{
    public int RequesterId { get; set; }
    public int EligibilityTypeId { get; set; }
}
