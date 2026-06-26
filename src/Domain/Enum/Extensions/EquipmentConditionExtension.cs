namespace RentalSystem.Domain.Enum.Extensions;

public static class EquipmentItemConditionExtension
{
    public static EquipmentItemCondition? ToCondition(this string condition)
    {
        switch (condition.ToLowerInvariant())
        {
            case "new":
                return EquipmentItemCondition.New;

            case "good":
                return EquipmentItemCondition.Good;

            case "fair":
                return EquipmentItemCondition.Fair;

            case "poor":
                return EquipmentItemCondition.Poor;

            case "damaged":
                return EquipmentItemCondition.Damaged;

            case "unusable":
                return EquipmentItemCondition.Unusable;

            default:
                return null;
        }
    }
}