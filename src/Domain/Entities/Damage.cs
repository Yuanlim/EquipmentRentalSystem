using RentalSystem.Domain.Common;

namespace RentalSystem.Domain.Entities;

public class Damage : RentalSystemDbBase
{
    private Damage() { }

    public Damage(
        Guid rentalOrderId,
        Guid rentalOrderItemId,
        string description,
        decimal fee,
        Guid equipmentItemId
    )
    {
        if (fee < 0)
            throw new InvalidOperationException("Damage fee cant be negative.");

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidOperationException("Damage description is required.");

        RentalOrderId = rentalOrderId;
        RentalOrderItemId = rentalOrderItemId;
        Description = description.Trim();
        Fee = fee;
        EquipmentItemId = equipmentItemId;

        MaintenanceJob = new(this);
    }

    public Guid RentalOrderId { get; private set; }

    public RentalOrder RentalOrder { get; private set; } = null!;

    public Guid RentalOrderItemId { get; private set; }

    public RentalOrderItem RentalOrderItem { get; private set; } = null!;

    public string Description { get; set; } = "";

    public decimal Fee { get; private set; }

    public bool Fixed { get; private set; }

    public Guid EquipmentItemId { get; private set; }

    public EquipmentItem EquipmentItem { get; private set; } = null!;

    public Guid MaintenanceJobId { get; private set; }

    public MaintenanceJob MaintenanceJob { get; private set; } = null!;
}