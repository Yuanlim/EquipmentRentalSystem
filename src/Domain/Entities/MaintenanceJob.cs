using RentalSystem.Domain.Common;
using RentalSystem.Domain.Enum;

namespace RentalSystem.Domain.Entities;

public class MaintenanceJob : RentalSystemDbBase
{
    // A maintenance job can be not pick up yet
    public Guid? TechnicianId { get; set; }

    // A maintenance job can handle many damages
    public required ICollection<Damage> Damages { get; set; }

    public MaintenanceStatus MaintenanceStatus { get; set; }

    public DateTime CreateAt { get; set; }

    // Is nullable because it possibly not yet completed
    public DateTime? CompletedAt { get; set; }

    public string Description { get; set; } = "";
}