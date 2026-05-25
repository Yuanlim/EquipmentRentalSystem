using RentalSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RentalSystem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using RentalSystem.Domain.Entities;
using System.Reflection;

namespace RentalSystem.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>, IRentalSystemDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<RentalOrder> RentalOrders { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Damage> Damages { get; set; }

    public DbSet<MaintenanceJob> MaintenanceJobs { get; set; }

    public DbSet<Inspection> Inspections { get; set; }

    public DbSet<RentalOrderItem> RentalOrderItems { get; set; }

    public DbSet<RentalStatusHistory> RentalStatusHistories { get; set; }

    public DbSet<EquipmentCategory> EquipmentCategories { get; set; }

    public DbSet<EquipmentItem> EquipmentItems { get; set; }

    public DbSet<Audit> Audits { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}