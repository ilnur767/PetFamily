using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Models;

namespace PetFamily.Framework.Authorization;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionRequirementHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAttribute requirement)
    {
        using var scope = _serviceProvider.CreateScope();
        var accountsContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();

        // var userIdString = context.User.Claims
        //     .FirstOrDefault(claim => claim.Type == CustomClaims.Id)?.Value;
        //
        // if (userIdString == null)
        // {
        //     context.Fail();
        //
        //     return;
        // }

        // if (!Guid.TryParse(userIdString, out Guid userId))
        // {
        //     context.Fail();
        //
        //     return;
        // }

        var permissions = context.User.Claims
            .Where(claim => claim.Type == CustomClaims.Permission)
            .Select(c => c.Value);

        if (permissions.Contains(requirement.Code))
        {
            context.Succeed(requirement);
        }
    }
}
