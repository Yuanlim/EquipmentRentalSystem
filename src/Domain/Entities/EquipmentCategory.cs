using RentalSystem.Domain.Common;

namespace RentalSystem.Domain.Entities;

public class EquipmentCategory : RentalSystemDbBase
{

    private EquipmentCategory() { }

    public EquipmentCategory(string name, string? createdByUserId, string assetTagPrefix)
    {
        (string trimmed, string normalized) = ToTrimmedAndNormalizedName(name);
        Name = trimmed;
        NormalizedName = normalized;
        CreatedByUserId = createdByUserId is null ? null : ValidateUserId(createdByUserId);
        CreatedAt = DateTime.UtcNow;
        SetAssetTagPrefix(assetTagPrefix);
    }

    // Used for original value
    public string Name { get; private set; } = "";

    // Used to see duplicate
    public string NormalizedName { get; private set; } = "";

    public string AssetTagPrefix { get; private set; } = "";

    public string? CreatedByUserId { get; init; } = "";

    public DateTime CreatedAt { get; init; }

    public ICollection<EquipmentItem> EquipmentItems { get; private set; } = [];

    public static (string, string) ToTrimmedAndNormalizedName(string categoryName)
    {
        ThrowIfIsNullOrWhiteSpace<EquipmentCategory>(categoryName);

        string trimmed = categoryName.Trim(); // "hEllo  world"

        // Ex: " hEllo  world  "
        return (trimmed, string.Join(" ",
            trimmed.Split(" ", StringSplitOptions.RemoveEmptyEntries) // "hEllo" ""[Removed] "world"
                    .Select(w => char.ToUpper(w[0]) + w[1..].ToLower() // "Hello World"
        )));
    }

    public void SetAssetTagPrefix(string assetTagPrefix)
    {
        if (!string.IsNullOrWhiteSpace(AssetTagPrefix))
            throw new InvalidOperationException($"{nameof(EquipmentCategory)} after an asset tag was set regarded to the category it shouldn't be set anymore to avoid old {nameof(EquipmentItem)} mismatch.");

        ThrowIfIsNullOrWhiteSpace<EquipmentCategory>(assetTagPrefix);

        AssetTagPrefix = assetTagPrefix.Trim().ToUpperInvariant();
    }

    public void AddEquipmentItem(EquipmentItem equipmentItem)
    {
        ThrowIfIsNull<EquipmentCategory, EquipmentItem>(equipmentItem);

        if (equipmentItem.CategoryId != Id)
            throw new InvalidOperationException("Equipment item does not belong to this category.");

        EquipmentItems.Add(equipmentItem);
    }
}