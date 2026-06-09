using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class EquipmentItem : RentalSystemDbBase
{
    private EquipmentItem() { }

    public EquipmentItem(
        decimal depositFee,
        decimal feePerDay,
        decimal lateFeePerDay,
        EquipmentCategory equipmentCategory,
        string description,
        string name,
        EquipmentItemCondition condition,
        EquipmentItemStatus status,
        string serialNumber,
        string? imagePath,
        string assetUniqueIdentifier
    )
    {
        // Domain rules
        EquipmentItemRuleSet(depositFee: depositFee, feePerDay: feePerDay, lateFeePerDay: lateFeePerDay, description: description, name: name, serialNumber: serialNumber, assetUniqueIdentifier: assetUniqueIdentifier);

        // Assign
        CategoryId = equipmentCategory.Id;
        EquipmentCategory = equipmentCategory;
        Condition = condition;
        Status = status;
        Description = description;
        Name = name;
        SerialNumber = serialNumber;
        ImagePath = imagePath;
        AssetTag = AssetUniqueIdentifierToTag(equipmentCategory, assetUniqueIdentifier);
    }

    // Counter get item initial fee
    public decimal DepositFee { get; private set; }

    // Rented each day how much should user paid
    public decimal FeePerDay { get; private set; }

    // Promised return date but return late, penalty fee
    public decimal LateFeePerDay { get; private set; }

    public Guid CategoryId { get; private set; }

    // Device category e.g. Laptop
    public EquipmentCategory EquipmentCategory { get; private set; } = null!;

    public string Description { get; private set; } = "";

    // Device name e.g. Microsoft surface 
    public string Name { get; private set; } = "";

    // Condition of the item
    public EquipmentItemCondition Condition { get; private set; }

    // Availability status
    public EquipmentItemStatus Status { get; private set; }

    // Model serial number
    public string SerialNumber { get; private set; } = "";

    // Product Image
    public string? ImagePath { get; private set; }

    // A distinguish name about this device (Whole company)
    public string AssetTag { get; private set; } = "";

    public void EquipmentItemRuleSet(
        decimal depositFee,
        decimal feePerDay,
        decimal lateFeePerDay,
        string description,
        string name,
        string serialNumber,
        string assetUniqueIdentifier
    )
    {
        if (depositFee < 0)
            throw new ArgumentException($"{nameof(EquipmentItem)} Deposit fee shouldn't be negative.");

        if (feePerDay < 0)
            throw new ArgumentException($"{nameof(EquipmentItem)} Fee per day shouldn't be negative.");

        if (lateFeePerDay < 0)
            throw new ArgumentException($"{nameof(EquipmentItem)} Late fee per day shouldn't be negative.");

        if (description.Length > 2000)
            throw new ArgumentException($"{nameof(EquipmentItem)} Description length shouldn't be larger than 2000.");

        if (name.Length > 200)
            throw new ArgumentException($"{nameof(EquipmentItem)} Name length shouldn't be larger than 200.");

        if (assetUniqueIdentifier.Length > 200)
            throw new ArgumentException($"{nameof(EquipmentItem)} Asset Tag length shouldn't be larger than 200.");

        if (serialNumber.Length > 200)
            throw new ArgumentException($"{nameof(EquipmentItem)} serial number length shouldn't be larger than 200.");

        // AssetTag should be category name followed by some unique identifier business defined:
        // {Category}-{unique identifier}
        if (string.IsNullOrWhiteSpace(assetUniqueIdentifier))
        {
            throw new ArgumentException($"{nameof(EquipmentItem)} assets unique identifier should be given; Because assets tag should be category asset tag prefix followed by some unique identifier. Ex:CAM-001, CAM-A001");
        }
    }

    public string AssetUniqueIdentifierToTag(EquipmentCategory equipmentCategory, string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            throw new ArgumentException($"{nameof(EquipmentItem)} cant convert to tag when identifier is null, empty or whitespace");
        }

        return equipmentCategory.AssetTagPrefix + "-" + identifier;
    }
}