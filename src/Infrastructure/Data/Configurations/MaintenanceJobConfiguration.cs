using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Domain.Entities;
using RentalSystem.Infrastructure.Identity;

namespace RentalSystem.Infrastructure.data.Configurations;

public class MaintenanceJobConfiguration : IEntityTypeConfiguration<MaintenanceJob>
{
	public void Configure(EntityTypeBuilder<MaintenanceJob> builder)
	{
		// Pk
		builder.HasKey(mj => mj.Id);

		// Indexing

		// Relation
		builder.HasOne(mj => mj.Damage)
				.WithOne(d => d.MaintenanceJob)
				.HasForeignKey<MaintenanceJob>(mj => mj.DamageId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne<ApplicationUser>()
				.WithMany()
				.HasForeignKey(mj => mj.TechnicianId)
				.OnDelete(DeleteBehavior.Restrict);

		// Property
		builder.Property(mj => mj.MaintenanceStatus)
				.HasConversion<string>()
				.IsRequired();

		builder.Property(mj => mj.CreatedAt)
				.IsRequired();

		builder.Property(mj => mj.Description)
				.HasMaxLength(1000);

		// Constraint
	}
}