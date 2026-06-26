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
        // Rules
        EquipmentItemRules(
            depositFee,
            feePerDay,
            lateFeePerDay,
            equipmentCategory,
            condition,
            status,
            description,
            name,
            serialNumber,
            assetUniqueIdentifier
        );


        // Assign
        DepositFee = depositFee;
        FeePerDay = feePerDay;
        LateFeePerDay = lateFeePerDay;
        CategoryId = equipmentCategory.Id;
        EquipmentCategory = equipmentCategory;
        Condition = condition;
        Status = status;
        Description = description;
        Name = name;
        SerialNumber = serialNumber;
        ImagePath = imagePath;
        AssetTag = AssetUniqueIdentifierToTag(equipmentCategory, assetUniqueIdentifier);

        equipmentCategory.AddEquipmentItem(this);
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

    public void EquipmentItemRules(
        decimal depositFee,
        decimal feePerDay,
        decimal lateFeePerDay,
        EquipmentCategory equipmentCategory,
        EquipmentItemCondition condition,
        EquipmentItemStatus status,
        string? description,
        string name,
        string serialNumber,
        string assetUniqueIdentifier
    )
    {
        ThrowIfLessThan<EquipmentItem>(depositFee, 0);
        ThrowIfLessThan<EquipmentItem>(feePerDay, 0);
        ThrowIfLessThan<EquipmentItem>(lateFeePerDay, 0);

        ThrowIfIsNull<EquipmentItem, EquipmentCategory>(equipmentCategory);
        ThrowIfIsNull<EquipmentItem, EquipmentItemCondition>(condition);
        ThrowIfIsNull<EquipmentItem, EquipmentItemStatus>(status);

        ThrowIfIsNullOrWhiteSpace<EquipmentItem>(name);
        ThrowIfIsNullOrWhiteSpace<EquipmentItem>(serialNumber);

        ThrowIfExceededLength<EquipmentItem>(description, 2000);
        ThrowIfExceededLength<EquipmentItem>(name, 200);
        ThrowIfExceededLength<EquipmentItem>(serialNumber, 200);

        if (string.IsNullOrWhiteSpace(assetUniqueIdentifier))
            throw new ArgumentException($"{nameof(EquipmentItem)} assets unique identifier should be given; Because assets tag should be category asset tag prefix followed by some unique identifier. Ex:CAM-001, CAM-A001");
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