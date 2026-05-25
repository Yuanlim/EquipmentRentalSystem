using RentalSystem.Domain.Common;

namespace RentalSystem.Domain.Entities;

public class RentalOrderItem : RentalSystemDbBase
{
    public Guid RentalOrderId { get; set; }

    public required RentalOrder RentalOrder { get; set; }

    public Guid EquipmentItemId { get; set; }

    public required EquipmentItem EquipmentItem { get; set; }

    public decimal FeePerDayAtRental { get; set; }

    public decimal DepositFeeAtRental { get; set; }
}