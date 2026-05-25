namespace RentalSystem.Domain.Enum;

public enum PaymentType
{
    RentalFee = 1,
    Deposit = 2,
    DamageFee = 3,
    LateFee = 4,
    Refund = 5,
}

public enum PaymentStatus
{
    Pending = 1,
    Paid = 2,
    Failed = 3,
    Refunded = 4,
    Cancelled = 5
}