using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class RentalOrder : RentalSystemDbBase
{
    private RentalOrder() { }

    public RentalOrder(
        DateOnly startDate,
        DateOnly endDate,
        string customerId,
        string? customerRequestNote = null)
    {
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date.");

        StartDate = startDate;
        EndDate = endDate;
        CustomerId = ValidateUserId(customerId);
        CustomerRequestNote = customerRequestNote ?? "";
        RentalStatus = RentalStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }

    public required string CustomerId { get; init; }

    public string CustomerRequestNote { get; private set; } = "";

    public RentalStatus RentalStatus { get; private set; }

    public DateTime? PickUpAt { get; private set; }

    public DateTime? ReturnedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public ICollection<Payment> Payments { get; private set; } = [];

    public ICollection<RentalOrderItem> Items { get; private set; } = [];

    public ICollection<Damage> Damages { get; private set; } = [];

    public ICollection<RentalStatusHistory> RentalStatusHistories { get; private set; } = [];

    // Order Flow
    // Sent => pending => staff => approved => customer paid total deposit => customer picked up => customer returned => paid for the rent fee and late fee => inspect => create damage fee => customer paid for the damage fee => completed the order

    /// <summary>
    /// <para>RentalOrder event handler</para>
    /// <para>Call this handler when customer picked up rented items</para>
    /// </summary>
    /// <param name="changedByUserId"></param>
    /// <param name="reason"></param>
    /// <exception cref="InvalidOperationException"></exception> 
    public void PickUpSetCurr(string changedByUserId, string? reason)
    {
        RentalStatus operationOf = RentalStatus.PickedUp;

        // An pickedUp event can be happen only in approved stage.
        InStage(RentalStatus.Approved, operationOf.ToText());

        // Clause
        Payment? depositPayment = Payments.FirstOrDefault(p => p.PaymentType == PaymentType.Deposit);

        if (depositPayment is null)
            throw new InvalidOperationException("No deposit payment tied to the order.");

        if (!PaymentPaid(depositPayment))
            throw new InvalidOperationException("Customer must pay the deposit before picking up items.");

        // TODO: Add Event Change Equipment rented

        AddNewRentalStatus(changedByUserId, reason, operationOf);
        PickUpAt = DateTime.UtcNow;
    }

    /// <summary>
    /// <para>RentalOrder event handler</para>
    /// <para>Call this handler customer returned rented items</para>
    /// </summary>
    /// <param name="changedByUserId"></param>
    /// <param name="reason"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void ReturnedSetCurr(string changedByUserId, string? reason)
    {
        RentalStatus operationOf = RentalStatus.Returned;

        // When returned the order should be in picked up stage
        InStage(RentalStatus.PickedUp, operationOf.ToText());

        // You need to pay all the Rental fee and Late fee before returned status.
        // The Order SHOULD have a rental fee in this stage
        bool hasRentalFee = false;

        foreach (var payment in Payments)
        {
            if (payment.PaymentType == PaymentType.RentalFee) // payment that are Rental fee
            {
                hasRentalFee = true;

                if (!PaymentPaidOrRefunded(payment)) // Not paid or refunded
                    throw new InvalidOperationException("The rental fee is not paid or refunded.");

                continue;
            }

            if (payment.PaymentType == PaymentType.LateFee)
            {
                if (!PaymentPaidOrRefunded(payment))
                    throw new InvalidOperationException("The late fee is not paid or refunded.");
            }
        }

        if (!hasRentalFee)
            throw new InvalidOperationException("The order has no rental fee but was accepted for return.");

        // TODO: Change Equipment status

        AddNewRentalStatus(changedByUserId, reason, operationOf);
        ReturnedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// <para>Rental order payment handler</para>
    /// <para>This added new damage fee to the order.</para>
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddNewRentalPayment()
    {
        // This action should always be in picked up stage
        InStage(RentalStatus.PickedUp, "adding new rental payment");

        // Can only be one rental fee existed
        Payment? payment = Payments.FirstOrDefault(p =>
            p.PaymentType == PaymentType.RentalFee ||
            p.PaymentType == PaymentType.LateFee
        );
        if (payment is not null)
            throw new InvalidOperationException(
                $"The rental fee or late fee has already existed. If you think it was a mistake consider modify Payment Id({payment.Id}) type or amount. Be aware that if customer paid already cant be changed again."
            );

        // Might returned earlier (cant use end date)
        int todaysNumber = DateOnly.FromDateTime(DateTime.Now).DayNumber;

        int days = Math.Min(EndDate.DayNumber, todaysNumber) - StartDate.DayNumber;
        int lateDays = todaysNumber - EndDate.DayNumber;

        decimal amount = 0;
        decimal lateAmount = 0;
        foreach (var i in Items)
        {
            // Calculate total
            amount += i.FeePerDayAtRental * days;
            lateAmount += i.LateFeePerDayAtRental * lateDays;
        }

        AddNewPayment(PaymentType.RentalFee, amount);
        if (lateAmount > 0) AddNewPayment(PaymentType.LateFee, lateAmount);
    }

    /// <summary>
    /// <para>Rental order payment handler</para>
    /// <para>This added new damage fee to the order.</para>
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <param name="description"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddNewDamagePayment(RentalOrderItem item, decimal amount, string description)
    {
        // This action should always be in returned stage
        InStage(RentalStatus.Returned, "adding new damage payment.");

        // See if this equipment existed in this order
        if (!Items.Any(i => i.Id == item.Id))
            throw new InvalidOperationException("User doesn't order this equipment item before.");

        // See if this equipment damage already been reported
        bool reported = Damages.Any(d => d.EquipmentItemId == item.EquipmentItemId);
        if (reported)
            throw new InvalidOperationException(
                "This equipment has been reported damage in this order. Each equipment damage in one order should reported them together. So when technician fixing them get all problem at once."
            );

        // Should have inspector before reporting damage
        // Should created damage, damage must create MaintenanceJob for technician
        // Should created payment
        Damage damage = new(Id, item.Id, description, amount, item.EquipmentItemId);
        Payment payment = item.CreateDamageFeePayment(amount);

        Payments.Add(payment);
    }

    /// <summary>
    /// <para>Rental order equipment handler</para>
    /// <para>This added new equipment item to the order.</para>
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddItems(RentalOrderItem item)
    {
        // This action should always be in pending stage
        InStage(RentalStatus.Pending, "adding new equipment item.");

        Items.Add(item);
    }

    /// <summary>
    /// Rental order equipment handler
    /// This added new equipment item to the order. 
    /// </summary>
    /// <param name="item"></param>
    public void AddItems(ICollection<RentalOrderItem> items)
    {
        // This action should always be in pending stage
        InStage(RentalStatus.Pending, "adding new equipment item.");

        foreach (var i in items)
        {
            Items.Add(i);
        }
    }

    // Internal used only methods

    private void AddNewPayment(PaymentType type, decimal amount)
    {
        Payments.Add(
            new(rentalOrderId: Id, paymentType: type, amount: amount)
        );
    }

    /// <summary>
    /// <para>Set new rental status</para>
    /// <para>Only RentalOrder event handler can call this method</para> 
    /// </summary>
    /// <param name="changedByUserId"></param>
    /// <param name="reason"></param>
    /// <param name="newRentalStatus"></param>
    private void AddNewRentalStatus(string changedByUserId, string? reason, RentalStatus newRentalStatus)
    {
        RentalStatusHistories.Add(
            new()
            {
                RentalOrderId = Id,
                ChangedByUserId = changedByUserId,
                OldStatus = RentalStatus,
                NewStatus = newRentalStatus,
                Reason = reason ?? ""
            }
        );
        RentalStatus = newRentalStatus;
    }

    /// <summary>
    /// Did this payment paid or refunded?
    /// </summary>
    /// <param name="payment"></param>
    /// <returns></returns>
    private static bool PaymentPaidOrRefunded(Payment payment)
    => payment.PaymentStatus is PaymentStatus.Paid or PaymentStatus.Refunded;

    /// <summary>
    /// Did this payment paid?
    /// </summary>
    /// <param name="payment"></param>
    /// <returns></returns>
    private static bool PaymentPaid(Payment payment)
    => payment.PaymentStatus == PaymentStatus.Paid;

    private void InStage(RentalStatus requiredStatus, string operation)
    {
        if (requiredStatus == RentalStatus)
            return;

        throw new InvalidOperationException(
            $"Only order with {requiredStatus} stage can be {operation}."
        );
    }
}