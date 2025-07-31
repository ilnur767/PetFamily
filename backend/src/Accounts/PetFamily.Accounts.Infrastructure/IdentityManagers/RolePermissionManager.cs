﻿using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class RolePermissionManager
{
    private readonly AccountsDbContext _accountsDbContext;

    public RolePermissionManager(AccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public async Task AddRangeIfExist(Guid roleId, IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var permission = await _accountsDbContext.Permissions.FirstOrDefaultAsync(p => p.Code == permissionCode);
            if (permission == null)
            {
                throw new ApplicationException($"Permission '{permissionCode}' not found");
            }

            var rolePermissionExist = await _accountsDbContext.RolePermissions.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);

            if (rolePermissionExist)
            {
                continue;
            }

            _accountsDbContext.RolePermissions.Add(
                new RolePermission { RoleId = roleId, PermissionId = permission!.Id });
        }

        await _accountsDbContext.SaveChangesAsync();
    }
}
