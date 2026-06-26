using Microsoft.EntityFrameworkCore;
using RentalSystem.Domain.Entities;

namespace RentalSystem.Application.Common.Interfaces;

public interface IRentalSystemDbContext
{
    DbSet<RentalOrder> RentalOrders { get; }

    DbSet<Payment> Payments { get; }

    DbSet<Damage> Damages { get; }

    DbSet<MaintenanceJob> MaintenanceJobs { get; }

    DbSet<Inspection> Inspections { get; }

    DbSet<RentalOrderItem> RentalOrderItems { get; }

    DbSet<RentalStatusHistory> RentalStatusHistories { get; }

    DbSet<EquipmentCategory> EquipmentCategories { get; }

    DbSet<EquipmentItem> EquipmentItems { get; }

    DbSet<Audit> Audits { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}