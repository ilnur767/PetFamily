using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.Entities;
using PetFamily.Volunteers.Domain.ValueObjects;
using static PetFamily.SharedKernel.Common.DataLimitsConstants;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Write;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(id => id.Value,
                value => PetId.Create(value));

        builder.Property(p => p.NickName).IsRequired().HasMaxLength(MaxLowTextLength);

        builder.OwnsOne(p => p.PetSpecies, species =>
        {
            species.Property(p => p.SpeciesId)
                .HasConversion(s => s.Value, s => SpeciesId.Create(s))
                .IsRequired()
                .HasColumnName("species_id");

            species.Property(p => p.BreedId)
                .HasConversion(b => b.Value, value => BreedId.Create(value))
                .IsRequired()
                .HasColumnName("breed_id");
        });

        builder.Property(p => p.Description).HasMaxLength(MaxHighTextLength);

        builder.Property(p => p.Color).HasMaxLength(MaxLowTextLength);
        builder.Property(p => p.HealthInformation).HasMaxLength(MaxLowTextLength);
        builder.Property(p => p.Address).HasMaxLength(MaxLowTextLength);
        builder.Property(p => p.Weight);
        builder.Property(p => p.Height);

        builder.OwnsOne(p => p.PhoneNumber, ph => { ph.Property(p => p.Value).HasColumnName("phone_number"); });

        builder.Property(p => p.IsCastrated).HasColumnName("is_castrated");
        builder.Property(p => p.DateOfBirth).HasColumnName("date_of_birth");
        builder.Property(p => p.CreatedAt).HasColumnName("created_at");
        builder.Property(p => p.IsVaccinated).HasColumnName("is_vaccinated");
        builder.Property(p => p.Status).HasMaxLength(MaxLowTextLength);

        builder.Property(p => p.Status)
            .HasConversion(p => p.Status.ToString(), p => PetStatus.Create(Enum.Parse<PetStatusValue>(p)))
            .HasColumnName("status").HasMaxLength(MaxLowTextLength);

        builder.Property(p => p.Requisites)
            .ValueObjectsCollectionJsonConversion(
                requisite => new RequisiteDto(requisite.Name, requisite.Description),
                dto => Requisite.Create(dto.Name, dto.Description).Value)
            .HasColumnName("requisites");

        builder.Property(p => p.Photos)
            .ValueObjectsCollectionJsonConversion(
                photo => new PhotoDto(photo.FileName, photo.FilePath, photo.IsMain),
                dto => Photo.Create(dto.FileName, dto.FilePath, dto.IsMain).Value)
            .HasColumnName("photos");

        builder.OwnsOne(p => p.Position, sr =>
        {
            sr.Property(s => s.Value)
                .IsRequired()
                .HasMaxLength(MaxLowTextLength)
                .HasColumnName("position");
        });
    }
}
