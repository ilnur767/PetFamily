using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");

        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");

        builder.HasOne(a => a.User)
            .WithOne(u=>u.AdminAccount)
            .HasForeignKey<AdminAccount>(a => a.UserId);
    }
}
