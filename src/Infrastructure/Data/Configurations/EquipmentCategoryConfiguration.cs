using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Domain.Entities;
using RentalSystem.Infrastructure.Identity;

namespace RentalSystem.Infrastructure.Data.Configurations;

public class EquipmentCategoryConfiguration : IEntityTypeConfiguration<EquipmentCategory>
{
	public void Configure(EntityTypeBuilder<EquipmentCategory> builder)
	{
		builder.HasKey(ec => ec.Id);

		// Indexing
		builder.HasIndex(ec => ec.Name)
				.IsUnique();

		builder.HasIndex(ec => ec.AssetTagPrefix)
				.IsUnique();

		// Relation
		// One admin can request a category creation
		builder.HasOne<ApplicationUser>()
				.WithMany()
				.HasForeignKey(ec => ec.CreatedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

		// Many equipment items can be the same category, category can be defining multiple items
		builder.HasMany(ec => ec.EquipmentItems)
				.WithOne(ei => ei.EquipmentCategory)
				.HasForeignKey(ei => ei.CategoryId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

		// Properties
		builder.Property(ec => ec.Name)
				.HasMaxLength(100)
				.IsRequired();

		builder.Property(ec => ec.NormalizedName)
				.HasMaxLength(100)
				.IsRequired();

		// Constraints
		builder.ToTable("EquipmentCategories", ec => ec.HasCheckConstraint(
			"CK_EquipmentCategory_NormalizedAndOriginalHasSameLength",
			@" LENGTH(""Name"") = LENGTH(""NormalizedName"") "
		));
	}
}