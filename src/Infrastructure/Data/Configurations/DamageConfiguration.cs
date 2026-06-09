using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Domain.Entities;

namespace RentalSystem.Infrastructure.Data.Configurations;

public class DamageConfiguration : IEntityTypeConfiguration<Damage>
{
        public void Configure(EntityTypeBuilder<Damage> builder)
        {
                builder.HasKey(d => d.Id); // Pk

                builder.HasIndex(d => d.RentalOrderId); // Indexing

                builder.HasIndex(d => d.EquipmentItemId)
                        .IsUnique() // Unique id when fixed == false
                        .HasFilter(@" ""Fixed"" = false ")
                        .HasDatabaseName("UX_Damages_EquipmentItemId_CantAddNewWhileOldNotResolved");

                builder.HasIndex(d => d.MaintenanceJobId);

                // One RentalOrder can cause multiple damages
                builder.HasOne(d => d.RentalOrder) // Fk
                        .WithMany(ro => ro.Damages)
                        .HasForeignKey(d => d.RentalOrderId)
                        .OnDelete(DeleteBehavior.Restrict);

                // One EquipmentItem can have multiple damage histories
                builder.HasOne(d => d.EquipmentItem)
                        .WithMany()
                        .HasForeignKey(d => d.EquipmentItemId)
                        .OnDelete(DeleteBehavior.Restrict);

                // One MaintenanceJob can handle multiple damages
                builder.HasOne(d => d.MaintenanceJob)
                        .WithOne()
                        .HasForeignKey<Damage>(d => d.MaintenanceJobId)
                        .OnDelete(DeleteBehavior.Restrict);

                // Fixed default is false
                builder.Property(d => d.Fixed)
                        .HasDefaultValue(false)
                        .IsRequired();

                // x*16.xx max number
                builder.Property(d => d.Fee)
                        .HasColumnType("decimal(18, 2)")
                        .IsRequired();

                // Max length in description
                builder.Property(d => d.Description)
                        .HasMaxLength(2500);

                // Fee negative handling
                builder.ToTable("Damages", d => d.HasCheckConstraint(
                    "CK_Damage_FeeCantBeNegative",
                    @" ""Fee"" > 0 "
                ));
        }
}