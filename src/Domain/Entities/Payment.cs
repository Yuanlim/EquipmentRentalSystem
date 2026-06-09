using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class Payment : RentalSystemDbBase
{
    private Payment() { }

    public Payment(
        Guid rentalOrderId,
        PaymentType paymentType,
        decimal amount
    )
    {
        RentalOrderId = rentalOrderId;
        PaymentType = paymentType;
        PaymentStatus = PaymentStatus.Pending;
        Amount = amount;
    }

    public Guid RentalOrderId { get; private set; }

    public RentalOrder RentalOrder { get; private set; } = null!;

    public PaymentType PaymentType { get; private set; }

    public PaymentStatus PaymentStatus { get; private set; }

    public decimal Amount { get; private set; }

    public DateTime? PaidAt { get; private set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}