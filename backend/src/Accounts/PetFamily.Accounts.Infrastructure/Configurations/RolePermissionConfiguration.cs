using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasOne(p => p.Role)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        builder.HasKey(p => new { p.RoleId, p.PermissionId });
    }
}
