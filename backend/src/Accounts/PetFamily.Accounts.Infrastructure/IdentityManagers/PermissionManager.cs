using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.DbContexts;

namespace PetFamily.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager
{
    private readonly AccountsDbContext _accountsDbContext;

    public PermissionManager(AccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public async Task AddRangeIfExist(IEnumerable<string> permissionsToAdd)
    {
        foreach (var permissionCode in permissionsToAdd)
        {
            var isPermissionExist = await _accountsDbContext.Permissions.AnyAsync(p => p.Code == permissionCode);

            if (isPermissionExist)
            {
                continue;
            }

            await _accountsDbContext.Permissions.AddAsync(new Permission { Code = permissionCode });
        }

        await _accountsDbContext.SaveChangesAsync();
    }

    public async Task<HashSet<string>> GetUserPermissions(Guid userId)
    {
        var permissions = await _accountsDbContext.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .SelectMany(r => r.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .ToListAsync();

        return permissions.ToHashSet();
    }
}
