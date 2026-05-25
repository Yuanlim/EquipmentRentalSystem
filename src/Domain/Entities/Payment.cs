using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class Payment : RentalSystemDbBase
{
    public Guid RentalOrderId { get; set; }

    public PaymentType PaymentType { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; }
}