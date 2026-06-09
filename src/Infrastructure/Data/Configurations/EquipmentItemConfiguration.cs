using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Domain.Entities;

namespace RentalSystem.Infrastructure.data.Configurations;

public class EquipmentItemConfiguration : IEntityTypeConfiguration<EquipmentItem>
{
	public void Configure(EntityTypeBuilder<EquipmentItem> builder)
	{
		// Pk
		builder.HasKey(ei => ei.Id);

		// Indexing
		builder.HasIndex(ei => ei.AssetTag)
				.IsUnique();

		// Relation
		builder.HasOne(ei => ei.EquipmentCategory)
				.WithMany(ec => ec.EquipmentItems)
				.HasForeignKey(ei => ei.CategoryId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

		// Property
		builder.Property(ei => ei.Condition)
				.HasConversion<string>();

		builder.Property(ei => ei.Status)
				.HasConversion<string>();

		builder.Property(ei => ei.Description)
				.HasMaxLength(2000);

		builder.Property(ei => ei.Name)
				.HasMaxLength(200)
				.IsRequired();

		builder.Property(ei => ei.SerialNumber)
				.HasMaxLength(200)
				.IsRequired();

		builder.Property(ei => ei.AssetTag)
				.HasMaxLength(200)
				.IsRequired();

		builder.Property(ei => ei.DepositFee)
				.HasColumnType("decimal(18, 2)")
				.IsRequired();

		builder.Property(ei => ei.FeePerDay)
				.HasColumnType("decimal(18, 2)")
				.IsRequired();

		builder.Property(ei => ei.LateFeePerDay)
				.HasColumnType("decimal(18, 2)")
				.IsRequired();

		// Constraint
		builder.ToTable(
			"EquipmentItems", ei => ei.HasCheckConstraint(
				"CK_EquipmentItems_DepositFeeCantBeNegativeOrZero",
				@"""DepositFee"" > 0"
			)
		);

		builder.ToTable(
			"EquipmentItems", ei => ei.HasCheckConstraint(
				"CK_EquipmentItems_LateFeePerDayCantBeNegativeOrZero",
				@"""LateFeePerDay"" > 0"
			)
		);

		builder.ToTable(
			"EquipmentItems", ei => ei.HasCheckConstraint(
				"CK_EquipmentItems_FeePerDayCantBeNegativeOrZero",
				@"""FeePerDay"" > 0"
			)
		);
	}
}