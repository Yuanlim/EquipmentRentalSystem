using RentalSystem.Domain.Common;

namespace RentalSystem.Domain.Entities;

public class EquipmentCategory : RentalSystemDbBase
{

    private EquipmentCategory() { }

    public EquipmentCategory(string name, string? createdByUserId, string assetTagPrefix)
    {
        Name = name.Trim();
        NormalizedName = ToNormalizedName(name);
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

    public static string ToNormalizedName(string category)
    {
        // Ex: " hEllo  world  "
        return string.Join(" ",
            category.Trim() // "hEllo  world"
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries) // "hEllo" ""[Removed] "world"
                    .Select(w => char.ToUpper(w[0]) + w[1..].ToLower() // "Hello World"
        ));
    }

    public void SetAssetTagPrefix(string assetTagPrefix)
    {
        if (!string.IsNullOrWhiteSpace(AssetTagPrefix))
            throw new InvalidOperationException($"{nameof(EquipmentCategory)} after an asset tag was set regarded to the category it shouldn't be set anymore to avoid old {nameof(EquipmentItem)} mismatch");

        if (string.IsNullOrWhiteSpace(assetTagPrefix))
            throw new ArgumentException($"{nameof(EquipmentCategory)} asset tag prefix shouldn't be whitespace, empty or null");

        AssetTagPrefix = assetTagPrefix.ToUpper();
    }
}