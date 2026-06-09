using RentalSystem.Domain.Common;

namespace RentalSystem.Domain.Entities;

public class Audit : RentalSystemDbBase
{
    public required string Action { get; init; }

    public string? EntityName { get; init; }

    public Guid? EntityId { get; init; }

    public string? OldValue { get; init; }

    public string? NewValue { get; init; }

    public string? PerformedByUserId { get; init; }

    public DateTime PerformedAt { get; init; }

    public CautionLevel CautionLevel { get; init; }

    public string? Route { get; init; }

    public string? HttpMethod { get; init; }
}