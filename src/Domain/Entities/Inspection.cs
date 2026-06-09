using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class Inspection : RentalSystemDbBase
{
    public Guid RentalOrderId { get; init; }

    public RentalOrder RentalOrder { get; init; } = null!;

    public Guid RentalOrderItemId { get; init; }

    public RentalOrderItem RentalOrderItem { get; init; } = null!;

    public required string TechnicianId { get; init; }

    public DateTime InspectionAt { get; init; } = DateTime.UtcNow;

    public string Comments { get; set; } = "";

    // Maybe not finish inspected
    public EquipmentItemCondition? DeterminedCondition { get; set; }
}