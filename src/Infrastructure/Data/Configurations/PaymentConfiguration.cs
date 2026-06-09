using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Domain.Entities;

namespace RentalSystem.Infrastructure.data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
	public void Configure(EntityTypeBuilder<Payment> builder)
	{
		// Pk
		builder.HasKey(p => p.Id);

		// Indexing

		// Relation
		builder.HasOne(p => p.RentalOrder)
				.WithMany(ro => ro.Payments)
				.HasForeignKey(p => p.RentalOrderId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

		// Property
		builder.Property(p => p.PaymentType)
				.IsRequired()
				.HasConversion<string>();

		builder.Property(p => p.PaymentStatus)
				.IsRequired()
				.HasConversion<string>();

		builder.Property(p => p.Amount)
				.IsRequired()
				.HasColumnType("decimal(18, 2)");

		// Constraints
		builder.ToTable("Payments", p => p.HasCheckConstraint(
			"CK_Payment_PaidAtCantBeBeforeCreatedAt",
			@" ""PaidAt"" >= ""CreatedAt"" "
		));

		builder.ToTable("Payments", p => p.HasCheckConstraint(
			"CK_Payment_AmountCantBeNegative",
			@" ""Amount"" >= 0 "
		));
	}
}