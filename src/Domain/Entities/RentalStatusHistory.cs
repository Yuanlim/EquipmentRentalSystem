using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class RentalStatusHistory : RentalSystemDbBase
{

    public Guid RentalOrderId { get; init; }

    public RentalOrder RentalOrder { get; set; } = null!;

    private DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public string? ChangedByUserId { get; init; }

    public RentalStatus? OldStatus { get; init; }

    public RentalStatus NewStatus { get; init; }

    public string Reason { get; init; } = "";

    public DateTime GetCreatedAt => CreatedAt;
}