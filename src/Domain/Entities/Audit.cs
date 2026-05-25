using RentalSystem.Domain.Common;

namespace RentalSystem.Domain.Entities;

public class Audit : RentalSystemDbBase
{
    public required string Action { get; set; }

    public string? EntityName { get; set; }

    public Guid? EntityId { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? PerformedByUserId { get; set; }

    public DateTime PerformedAt { get; set; }

    public CautionLevel CautionLevel { get; set; }

    public string? Route { get; set; }

    public string? HttpMethod { get; set; }
}