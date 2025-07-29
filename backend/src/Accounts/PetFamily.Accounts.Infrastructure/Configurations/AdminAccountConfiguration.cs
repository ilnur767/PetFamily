using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");

        builder.HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<AdminAccount>(a => a.UserId);

        builder.ComplexProperty(a => a.FullName, fb =>
        {
            fb.Property(a => a.FirstName).HasColumnName("first_name").IsRequired();
            fb.Property(a => a.LastName).HasColumnName("last_name").IsRequired();
            fb.Property(a => a.MiddleName).HasColumnName("middle_name").IsRequired();
        });
    }
}
