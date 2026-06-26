namespace RentalSystem.Domain.Enum.Extensions;

public static class EquipmentStatusExtension
{
    public static EquipmentItemStatus? ToStatus(this string status)
    {
        switch (status.ToLowerInvariant())
        {
            case "retired":
                return EquipmentItemStatus.Retired;

            case "available":
                return EquipmentItemStatus.Available;

            case "reserved":
                return EquipmentItemStatus.Reserved;

            case "rented":
                return EquipmentItemStatus.Rented;

            case "undermaintenance":
                return EquipmentItemStatus.UnderMaintenance;

            default:
                return null;
        }
    }
}