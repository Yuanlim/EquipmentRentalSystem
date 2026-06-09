using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class MaintenanceJob : RentalSystemDbBase
{
    private MaintenanceJob() { }

    public MaintenanceJob(Damage damage)
    {
        DamageId = damage.Id;
        Damage = damage;
        MaintenanceStatus = MaintenanceStatus.Pending;
    }

    // A maintenance job can be not pick up yet
    public string? TechnicianId { get; private set; }

    // A maintenance job can handle one damage
    public Guid DamageId { get; private set; }

    public Damage Damage { get; private set; } = null!;

    public MaintenanceStatus MaintenanceStatus { get; private set; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    // Is nullable because it possibly not yet completed
    public DateTime? CompletedAt { get; private set; }

    public string Description { get; private set; } = "";

    public void CreateMaintenanceJob(Damage damage)
    {
        if (damage.Fixed)
            throw new InvalidOperationException("This damage already been fixed.");

        TechnicianId = null;
        DamageId = damage.Id;
        MaintenanceStatus = MaintenanceStatus.Pending;
    }
}