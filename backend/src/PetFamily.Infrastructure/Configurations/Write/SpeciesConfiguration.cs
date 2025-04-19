﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Specieses;
using static PetFamily.Domain.Common.DataLimitsConstants;

namespace PetFamily.Infrastructure.Configurations.Write;

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
