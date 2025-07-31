using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace PetFamily.Framework.Authorization;

public sealed class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (string.IsNullOrWhiteSpace(policyName))
        {
            return Task.FromResult<AuthorizationPolicy?>(null);
        }

        var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionAttribute(policyName))
            .Build();

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult(new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return Task.FromResult<AuthorizationPolicy?>(null);
    }
}
