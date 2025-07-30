using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Dtos;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<IdentityUserRole<Guid>>();

        builder
            .HasOne(u => u.AdminAccount)
            .WithOne()
            .HasForeignKey<AdminAccount>(x=>x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(u => u.VolunteerAccount)
            .WithOne()
            .HasForeignKey<VolunteerAccount>(x=>x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(u => u.ParticipantAccount)
            .WithOne()
            .HasForeignKey<ParticipantAccount>(x=>x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(u => u.SocialMedias)
            .ValueObjectsCollectionJsonConversion(
                socialMedia => new SocialMediaDto(socialMedia.Name, socialMedia.Link),
                dto => SocialMedia.Create(dto.Name, dto.Link).Value);

        builder.ComplexProperty(a => a.FullName, fb =>
        {
            fb.Property(a => a.FirstName).HasColumnName("first_name").IsRequired();
            fb.Property(a => a.LastName).HasColumnName("last_name").IsRequired();
            fb.Property(a => a.MiddleName).HasColumnName("middle_name").IsRequired();
        });
    }
}
