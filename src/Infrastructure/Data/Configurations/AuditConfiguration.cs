using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalSystem.Domain.Entities;

namespace RentalSystem.Infrastructure.Data.Configurations;

public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.HasKey(a => a.Id); // Pk

        builder.Property(a => a.CautionLevel)
                .HasConversion<string>(); // CautionLevel saved as string

        builder.Property(a => a.HttpMethod)
                .HasMaxLength(10);

        builder.Property(a => a.EntityName)
                .HasMaxLength(100);

        builder.Property(a => a.Route)
                .HasMaxLength(100);

        builder.ToTable("Audits", a => a.HasCheckConstraint(
            "OldAndNewCantBeSameValue",
            @" ""OldValue"" IS NULL OR ""NewValue"" IS NULL OR ""OldValue"" <> ""NewValue"" "
        ));

        builder.ToTable("Audits", a => a.HasCheckConstraint(
            "PerformedAtCantBeFuture",
            @" ""PerformedAt"" <= ""CURRENT_TIMESTAMP"" "
        ));
    }
}