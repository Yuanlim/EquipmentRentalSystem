using RentalSystem.Domain.Common;

namespace RentalSystem.Domain.Entities;

public class EquipmentCategory : RentalSystemDbBase
{
    // Used for original value
    public string Name { get; set; } = "";

    // Used to see duplicate
    public string NormalizedName { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public Guid CreatedByUserId { get; set; }
}