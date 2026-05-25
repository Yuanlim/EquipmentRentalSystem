using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class Inspection : RentalSystemDbBase
{
    public Guid RentalOrderId { get; set; }

    public Guid RentalOrderItemId { get; set; }

    public Guid TechnicianId { get; set; }

    public DateTime InspectionAt { get; set; }

    public string Comments { get; set; } = "";

    public EquipmentItemCondition DeterminedCondition { get; set; }
}