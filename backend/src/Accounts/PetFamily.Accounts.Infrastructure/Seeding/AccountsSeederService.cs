using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Models;
using PetFamily.Framework;

namespace PetFamily.Accounts.Infrastructure.Seeding;

public class AccountsSeederService
{
    private readonly AdminOptions _adminOptions;
    private readonly ILogger<AccountsSeederService> _logger;
    private readonly PermissionManager _permissionManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly RolePermissionManager _rolePermissionManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public AccountsSeederService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        PermissionManager permissionManager,
        RolePermissionManager rolePermissionManager,
        IOptions<AdminOptions> adminOptions,
        ILogger<AccountsSeederService> logger,
        [FromKeyedServices(UnitOfWorkTypes.Accounts)]
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _permissionManager = permissionManager;
        _rolePermissionManager = rolePermissionManager;
        _adminOptions = adminOptions.Value;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Seeding accounts...");

        var json = await File.ReadAllTextAsync(JsonPaths.Accounts);

        var seedData = JsonSerializer.Deserialize<RolePermissionOptions>(json)
                       ?? throw new ApplicationException("Could not deserialize role permission config");

        await SeedPermissions(seedData);

        await SeedRoles(seedData);

        await SeedRolePermissions(seedData);

        await CreateAdminIfNotExists(cancellationToken);
    }

    private async Task CreateAdminIfNotExists(CancellationToken cancellationToken)
    {
        var existsAdmin = await _userManager.FindByNameAsync(_adminOptions.UserName);

        if (existsAdmin != null)
        {
            return;
        }

        var adminUser = User.CreateAdmin(_adminOptions.UserName, _adminOptions.Email);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var adminResult = await _userManager.CreateAsync(adminUser, _adminOptions.Password);

            if (!adminResult.Succeeded)
            {
                _logger.LogError(JsonSerializer.Serialize(adminResult.Errors));
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw new ApplicationException("Could not create admin");
            }

            var roleReuslt = await _userManager.AddToRoleAsync(adminUser, AdminAccount.ADMIN);

            if (!roleReuslt.Succeeded)
            {
                _logger.LogError(JsonSerializer.Serialize(roleReuslt.Errors));
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw new ApplicationException("Could not create admin role");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
        }
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
