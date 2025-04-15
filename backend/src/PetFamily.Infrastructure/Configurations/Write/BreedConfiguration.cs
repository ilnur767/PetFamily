using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Specieses;
using static PetFamily.Domain.Common.DataLimitsConstants;

namespace PetFamily.Infrastructure.Configurations.Write;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(x => x.Id);

        builder.Property(b => b.Id)
            .HasConversion(b => b.Value, value => BreedId.Create(value));

        builder.Property(b => b.Name).HasMaxLength(MaxLowTextLength);
    }
}
