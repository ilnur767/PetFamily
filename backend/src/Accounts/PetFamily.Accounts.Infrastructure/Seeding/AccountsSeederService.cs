using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Framework;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeederService
{
    private readonly AdminAccountManager _adminAccountManager;
    private readonly AdminOptions _adminOptions;
    private readonly ILogger<AccountsSeederService> _logger;
    private readonly PermissionManager _permissionManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly RolePermissionManager _rolePermissionManager;
    private readonly UserManager<User> _userManager;

    public AccountsSeederService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        PermissionManager permissionManager,
        RolePermissionManager rolePermissionManager,
        IOptions<AdminOptions> adminOptions,
        ILogger<AccountsSeederService> logger, AdminAccountManager adminAccountManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _permissionManager = permissionManager;
        _rolePermissionManager = rolePermissionManager;
        _adminOptions = adminOptions.Value;
        _logger = logger;
        _adminAccountManager = adminAccountManager;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding accounts...");

        var json = await File.ReadAllTextAsync(JsonPaths.Accounts);

        var seedData = JsonSerializer.Deserialize<RolePermissionOptions>(json)
                       ?? throw new ApplicationException("Could not deserialize role permission config");

        await SeedPermissions(seedData);

        await SeedRoles(seedData);

        await SeedRolePermissions(seedData);

        await CreateAdminIfNotExists();
    }

    private async Task CreateAdminIfNotExists()
    {
        var existsAdmin = await _userManager.FindByNameAsync(_adminOptions.UserName);

        if (existsAdmin != null)
        {
            return;
        }

        var role = await _roleManager.FindByNameAsync(AdminAccount.ADMIN)
                   ?? throw new ApplicationException("Could not find admin role");

        var adminUser = User.CreateAdmin(_adminOptions.UserName, _adminOptions.Email, role);
        await _userManager.CreateAsync(adminUser, _adminOptions.Password);

        var fullName = FullName.Create(_adminOptions.UserName, _adminOptions.UserName, _adminOptions.UserName).Value;
        var adminAccount = new AdminAccount(fullName, adminUser);
        await _adminAccountManager.CreateAdminAccount(adminAccount);
    }

    private async Task SeedRolePermissions(RolePermissionOptions seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            var rolePermissions = seedData.Roles[roleName];
            await _rolePermissionManager.AddRangeIfExist(role!.Id, rolePermissions);
        }
    }

    private async Task SeedRoles(RolePermissionOptions seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var existingRole = await _roleManager.FindByNameAsync(roleName);

            if (existingRole is null)
            {
                await _roleManager.CreateAsync(new Role { Name = roleName });
            }
        }

        _logger.LogInformation("Roles add to database.");
    }

    private async Task SeedPermissions(RolePermissionOptions seedData)
    {
        var permissionsToAdd = seedData.Permissions.SelectMany(p => p.Value);

        await _permissionManager.AddRangeIfExist(permissionsToAdd);

        _logger.LogInformation("Permissions add to database.");
    }
}
