using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Infrastructure.DbContexts;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuthorizationDbContext>();

        services.AddTransient<ITokenProvider, JwtTokenProvider>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));

        return services;
    }
}
