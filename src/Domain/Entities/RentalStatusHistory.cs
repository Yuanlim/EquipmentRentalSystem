using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class RentalStatusHistory : RentalSystemDbBase
{
    public Guid RentalOrderId { get; set; }

    public required RentalOrder RentalOrder { get; set; }

    public Guid? ChangedByUserId { get; set; }

    public DateTime CreatedAt
    { get; set; }

    public RentalStatus? OldStatus { get; set; }

    public RentalStatus NewStatus { get; set; }

    public string Reason { get; set; } = "";
}