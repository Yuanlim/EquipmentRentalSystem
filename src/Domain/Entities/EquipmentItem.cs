using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class EquipmentItem : RentalSystemDbBase
{
    // Counter get item initial fee
    public decimal DepositFee { get; set; }

    // Rented each day how much should user paid
    public decimal FeePerDay { get; set; }

    // Promised return date but return late, penalty fee
    public decimal LateFeePerDay { get; set; }

    public Guid CategoryId { get; set; }

    // Device category e.g. Laptop
    public required EquipmentCategory EquipmentCategory { get; set; }

    public string Description { get; set; } = "";

    // Device name e.g. Microsoft surface 
    public string Name { get; set; } = "";

    // Condition of the item
    public EquipmentItemCondition Condition { get; set; }

    // Availability status
    public EquipmentItemStatus Status { get; set; }

    // Model serial number
    public required string SerialNumber { get; set; }

    // Product Image
    public string? ImagePath { get; set; }

    // A distinguish name about this device (Whole company)
    public required string AssetTag { get; set; }
}