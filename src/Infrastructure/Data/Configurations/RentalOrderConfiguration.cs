using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Domain.Entities;
using RentalSystem.Infrastructure.Identity;

namespace RentalSystem.Infrastructure.Data.Configurations;

public class RentalOrderConfiguration : IEntityTypeConfiguration<RentalOrder>
{
    public void Configure(EntityTypeBuilder<RentalOrder> builder)
    {
        builder.HasKey(ro => ro.Id);

        // One CustomerId can have only one
        // Pending, PickedUp, Approved, Returned order.
        builder.HasIndex(ro => ro.CustomerId)
                .IsUnique()
                .HasFilter(@" ""RentalStatus"" IN ('Pending', 'PickedUp', 'Approved', 'Returned')")
                .HasDatabaseName("UX_RentalOrders_CustomerId_CanOnlyHasOneUnfinishedOrder");

        // Relations
        builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(ro => ro.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ro => ro.Damages)
                .WithOne()
                .HasForeignKey(d => d.RentalOrderId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ro => ro.Payments)
                .WithOne()
                .HasForeignKey(p => p.RentalOrderId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ro => ro.RentalStatusHistories)
                .WithOne()
                .HasForeignKey(rsh => rsh.RentalOrderId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(ro => ro.Items)
                .WithOne()
                .HasForeignKey(i => i.RentalOrderId)
                .OnDelete(DeleteBehavior.Restrict);

        // Property configuration
        builder.Property(ro => ro.RentalStatus)
                .HasConversion<string>();

        builder.Property(ro => ro.CustomerRequestNote)
                .HasMaxLength(500);

        // Constraints
        builder.ToTable("RentalOrders", ro =>
        {
            ro.HasCheckConstraint(
                "CK_RentalOrder_StartDateBeforeEndDate",
                @" ""StartDate"" <= ""EndDate"" "
            );

            ro.HasCheckConstraint(
                "CK_RentalOrder_StartDateAfterToday",
                @" ""StartDate"" >= CURRENT_DATE "
            );

            ro.HasCheckConstraint(
                "CK_RentalOrder_PickUpAtBeforeReturnedAt",
                @" ""PickUpAt"" IS NULL OR ""ReturnedAt"" IS NULL OR ""PickUpAt"" <= ""ReturnedAt"" "
            );

            // Never pick or return but completed?
            ro.HasCheckConstraint(
                "CompletedNeedPickUpAndReturn",
                @" ""RentalStatus"" <> Completed OR (""PickUpAt"" IS NOT NULL AND ""ReturnedAt"" IS NOT NULL) "
            );
        });
    }
}