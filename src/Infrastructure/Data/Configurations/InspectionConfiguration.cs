using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Domain.Entities;
using RentalSystem.Infrastructure.Identity;

namespace RentalSystem.Infrastructure.data.Configurations;

public class InspectionConfiguration : IEntityTypeConfiguration<Inspection>
{
	public void Configure(EntityTypeBuilder<Inspection> builder)
	{
		// Pk
		builder.HasKey(i => i.Id);

		// Indexing
		builder.HasIndex(i => new { i.RentalOrderId, i.RentalOrderItemId })
				.IsUnique();

		// Relation
		builder.HasOne<ApplicationUser>()
				.WithMany()
				.HasForeignKey(au => au.TechnicianId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(i => i.RentalOrder)
				.WithMany()
				.HasForeignKey(i => i.RentalOrderId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(i => i.RentalOrderItem)
				.WithMany()
				.HasForeignKey(i => i.RentalOrderItemId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

		// Property
		builder.Property(i => i.InspectionAt)
				.IsRequired();

		builder.Property(i => i.TechnicianId)
				.IsRequired();

		builder.Property(i => i.Comments)
				.HasMaxLength(1000);

		builder.Property(i => i.DeterminedCondition)
				.HasConversion<string>();

		// Constraint
	}
}