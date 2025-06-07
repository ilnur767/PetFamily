using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Specieses.Domain.Specieses;
using static PetFamily.SharedKernel.Common.DataLimitsConstants;

namespace PetFamily.Specieses.Infrastructure.Configurations.Write;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("specieses");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, value => SpeciesId.Create(value));

        builder.Property(s => s.Name).IsRequired().HasMaxLength(MaxLowTextLength);

        builder.HasMany(s => s.Breeds).WithOne().HasForeignKey("species_id");
    }
}
