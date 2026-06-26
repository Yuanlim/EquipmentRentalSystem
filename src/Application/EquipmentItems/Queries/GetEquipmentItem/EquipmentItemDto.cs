using RentalSystem.Domain.Entities;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Application.EquipmentItems.Queries.GetEquipmentItem;

public class EquipmentItemDto
{
    public required string Description { get; init; }
    public required EquipmentCategory EquipmentCategory { get; init; }
    public required string Name { get; init; }
    public required EquipmentItemCondition Condition { get; init; }
    public required EquipmentItemStatus Status { get; init; }
    public decimal DepositFee { get; init; }
    public decimal FeePerDay { get; init; }
    public decimal LateFeePerDay { get; init; }
}

public static class EquipmentItemExtension
{
    public static EquipmentItemDto ToDto(this EquipmentItem equipmentItem)
    => new()
    {
        Description = equipmentItem.Description,
        EquipmentCategory = equipmentItem.EquipmentCategory,
        Name = equipmentItem.Name,
        Condition = equipmentItem.Condition,
        Status = equipmentItem.Status,
        DepositFee = equipmentItem.DepositFee,
        FeePerDay = equipmentItem.FeePerDay,
        LateFeePerDay = equipmentItem.LateFeePerDay
    };
}