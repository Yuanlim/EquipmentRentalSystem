namespace RentalSystem.Domain.Enum;

public enum EquipmentItemCondition
{
    New = 1,
    Good = 2,
    Fair = 3,
    Poor = 4,
    Damaged = 5,
    Unusable = 6
}

public enum EquipmentItemStatus
{
    Available = 1,
    Reserved = 2,
    Rented = 3,
    UnderMaintenance = 4,
    Retired = 5
}