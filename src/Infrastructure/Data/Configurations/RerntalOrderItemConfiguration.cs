using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Domain.Entities;

namespace RentalSystem.Infrastructure.data.Configurations;

public class RentalOrderItemConfiguration : IEntityTypeConfiguration<RentalOrderItem>
{
    public void Configure(EntityTypeBuilder<RentalOrderItem> builder)
    {
        // Pk
        builder.HasKey(roi => roi.Id);

        // Indexing
        builder.HasIndex(roi => new { roi.EquipmentItemId, roi.RentalOrderId })
                .IsUnique();

        builder.HasIndex(roi => roi.InspectionId)
                .IsUnique();

        // Relation
        builder.HasOne(roi => roi.RentalOrder)
                .WithMany(ro => ro.Items)
                .HasForeignKey(roi => roi.RentalOrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(roi => roi.EquipmentItem)
                .WithMany()
                .HasForeignKey(roi => roi.EquipmentItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(roi => roi.Inspection)
                .WithOne(i => i.RentalOrderItem)
                .HasForeignKey<RentalOrderItem>(roi => roi.InspectionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        // Property
        builder.Property(roi => roi.DepositFeeAtRental)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

        builder.Property(roi => roi.FeePerDayAtRental)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

        // Constraint
        builder.ToTable(
            "RentalOrderItems", ei => ei.HasCheckConstraint(
                "CK_RentalOrderItems_DepositFeeAtRentalCantBeNegativeOrZero",
                @"""DepositFeeAtRental"" > 0"
            )
        );

        builder.ToTable(
            "RentalOrderItems", ei => ei.HasCheckConstraint(
                "CK_RentalOrderItems_FeePerDayAtRentalCantBeNegativeOrZero",
                @"""FeePerDayAtRental"" > 0"
            )
        );

        builder.ToTable(
            "RentalOrderItems", ei => ei.HasCheckConstraint(
                "CK_RentalOrderItems_LateFeePerDayAtRentalCantBeNegativeOrZero",
                @"""LateFeePerDayAtRental"" > 0"
            )
        );
    }
}