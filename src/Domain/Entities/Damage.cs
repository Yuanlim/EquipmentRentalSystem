using RentalSystem.Domain.Common;

namespace RentalSystem.Domain.Entities;

public class Damage : RentalSystemDbBase
{
    public Guid RentalOrderId { get; set; }

    public string Description { get; set; } = "";

    public decimal Fee { get; set; }

    public bool Fixed { get; set; }

    public Guid EquipmentItemId { get; set; }

    public required EquipmentItem EquipmentItem { get; set; }

    public Guid MaintenanceJobId { get; set; }

    public required MaintenanceJob MaintenanceJob { get; set; }
}