using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class RentalOrder : RentalSystemDbBase
{
    public DateOnly StartDate { get; set; }

    public DateOnly EndingDate { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerRequestNote { get; set; } = "";

    public RentalStatus RentalStatus { get; set; }

    public DateTime? PickUpAt { get; set; }

    public DateTime? ReturnAt { get; set; }

    public required ICollection<Payment> Payments { get; set; }

    public required ICollection<RentalOrderItem> Items { get; set; }

    public ICollection<Damage> Damages { get; set; } = [];

    public ICollection<RentalStatusHistory> RentalStatusHistories = [];
}