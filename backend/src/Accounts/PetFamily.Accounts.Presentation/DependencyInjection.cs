using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Contracts;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Framework.Authorization;

namespace PetFamily.Accounts.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>()
                                 ?? throw new ApplicationException($"Missing {JwtOptions.Jwt} in configuration");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization();

        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        services.AddIdentity<User, Role>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AccountsDbContext>();

        return services.AddScoped<IAccountsContract, AccountsContract>();

        return services;
    }
}
