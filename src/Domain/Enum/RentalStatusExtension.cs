namespace RentalSystem.Domain.Enum;

public static class RentalStatusExtension
{
    /// <summary>
    /// Normal rental status enum to string
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public static string ToText(this RentalStatus status)
    {
        return status switch
        {
            RentalStatus.Pending => "pending",
            RentalStatus.Approved => "approved",
            RentalStatus.Rejected => "rejected",
            RentalStatus.Cancelled => "cancelled",
            RentalStatus.PickedUp => "picked up",
            RentalStatus.Returned => "returned",
            RentalStatus.Completed => "completed",
            _ => ""
        };
    }
}