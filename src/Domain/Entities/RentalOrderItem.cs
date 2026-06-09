using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class RentalOrderItem : RentalSystemDbBase
{
    private RentalOrderItem() { }

    public RentalOrderItem(
        Guid rentalOrderId,
        Guid equipmentItemId,
        decimal feePerDayAtRental,
        decimal lateFeePerDayAtRental,
        decimal depositFeeAtRental
    )
    {
        RentalOrderId = rentalOrderId;
        EquipmentItemId = equipmentItemId;
        FeePerDayAtRental = feePerDayAtRental;
        LateFeePerDayAtRental = lateFeePerDayAtRental;
        DepositFeeAtRental = depositFeeAtRental;
    }

    public Guid RentalOrderId { get; init; }

    public RentalOrder RentalOrder { get; private set; } = null!;

    public Guid EquipmentItemId { get; private set; }

    public EquipmentItem EquipmentItem { get; private set; } = null!;

    // Once set should not be able to set
    public Guid? InspectionId { get; private set; }

    public Inspection? Inspection { get; private set; }

    public decimal FeePerDayAtRental { get; private set; }

    public decimal DepositFeeAtRental { get; private set; }

    public decimal LateFeePerDayAtRental { get; private set; }

    public void SetInspectionId(Guid id)
    {
        if (InspectionId is not null)
            throw new("Inspection already been handled");

        InspectionId = id;
    }

    public Payment CreateDamageFeePayment(decimal amount)
    {
        if (InspectionId is null)
            throw new("Inspection hasn't been done before report damage.");

        if (amount <= 0)
            throw new InvalidOperationException("Damage fee amount must be greater than zero.");

        return new(
            rentalOrderId: RentalOrderId,
            paymentType: PaymentType.DamageFee,
            amount: amount
        );
    }
}