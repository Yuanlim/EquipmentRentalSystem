using RentalSystem.Domain.Enum;

public class EquipmentCategoryTestData
{
    public string Name { get; set; } = "";
    public string? CreatedByUserId { get; set; }
    public string AssetTagPrefix { get; set; } = "";
}

public class EquipmentItemTestData
{
    public decimal DepositFee { get; set; }
    public decimal FeePerDay { get; set; }
    public decimal LateFeePerDay { get; set; }
    public string Description { get; set; } = "";
    public string Name { get; set; } = "";
    public EquipmentItemCondition Condition { get; set; }
    public EquipmentItemStatus Status { get; set; }
    public string SerialNumber { get; set; } = "";
    public string? ImagePath { get; set; }
    public string AssetUniqueIdentifier { get; set; } = "";
    public string AssetTag { get; set; } = "";
}