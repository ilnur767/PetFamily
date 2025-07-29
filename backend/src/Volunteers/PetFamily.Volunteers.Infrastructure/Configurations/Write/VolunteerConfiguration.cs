using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.Entities;
using static PetFamily.SharedKernel.Common.DataLimitsConstants;

namespace PetFamily.Volunteers.Infrastructure.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(id => id.Value,
                value => VolunteerId.Create(value));

        builder.OwnsOne(v => v.FullName, name =>
        {
            name.Property(n => n.FirstName)
                .IsRequired()
                .HasColumnName("first_name")
                .HasMaxLength(MaxLowTextLength);

            name.Property(n => n.LastName)
                .IsRequired()
                .HasColumnName("last_name")
                .HasMaxLength(MaxLowTextLength);

            name.Property(n => n.MiddleName)
                .IsRequired()
                .HasColumnName("middle_name")
                .HasMaxLength(MaxLowTextLength);
        });

        builder.OwnsOne(v => v.Email, email => { email.Property(n => n.Address).HasMaxLength(MaxLowTextLength); });

        builder.Property(v => v.Description)
            .HasMaxLength(MaxHighTextLength);

        builder.Property(v => v.WorkExperience);

        builder.OwnsOne(v => v.PhoneNumber, ph =>
        {
            ph.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("phone_number")
                .HasMaxLength(MaxLowTextLength);
        });

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id");

        builder.Property(v => v.IsDeleted).HasColumnName("is_deleted");
        builder.Property(v => v.DeletedAt).HasColumnName("deleted_at");
    }
}
