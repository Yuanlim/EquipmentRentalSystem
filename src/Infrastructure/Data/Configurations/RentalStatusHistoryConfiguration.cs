using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Domain.Entities;
using RentalSystem.Infrastructure.Identity;

namespace RentalSystem.Infrastructure.data.Configurations;

public class RentalStatusHistoryConfiguration : IEntityTypeConfiguration<RentalStatusHistory>
{
	public void Configure(EntityTypeBuilder<RentalStatusHistory> builder)
	{
		// Pk
		builder.HasKey(rsh => rsh.Id);

		// Indexing

		// Relation
		builder.HasOne(rsh => rsh.RentalOrder)
				.WithMany(ro => ro.RentalStatusHistories)
				.HasForeignKey(rsh => rsh.RentalOrderId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne<ApplicationUser>()
				.WithMany()
				.HasForeignKey(rsh => rsh.ChangedByUserId)
				.OnDelete(DeleteBehavior.Restrict);

		// Properties
		builder.Property(rsh => rsh.Reason)
				.HasMaxLength(1000);

		builder.Property(rsh => rsh.OldStatus)
				.HasConversion<string>();

		builder.Property(rsh => rsh.NewStatus)
				.IsRequired()
				.HasConversion<string>();

		// Constraints
		builder.ToTable("RentalStatusHistories", rsh => rsh.HasCheckConstraint(
			"CK_RentalOrderHistory_OldAndNewValueCantBeSame",
			@" ""OldStatus"" IS NULL OR ""OldStatus"" <> ""NewStatus"" "
		));
	}
}