using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Common;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");

        builder.HasOne(v => v.User)
            .WithOne()
            .HasForeignKey<VolunteerAccount>(v => v.UserId);

        builder.Property(v => v.Certificates)
            .HasMaxLength(DataLimitsConstants.MaxHighTextLength);

        builder.Property(u => u.Requisites)
            .ValueObjectsCollectionJsonConversion(
                socialMedia => new RequisiteDto(socialMedia.Name, socialMedia.Description),
                dto => Requisite.Create(dto.Name, dto.Description).Value);
    }
}
