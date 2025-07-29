using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.Abstractions;
using PetFamily.Accounts.Application.Providers;
using PetFamily.Accounts.Infrastructure.DbContexts;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Seeding;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AccountsDbContext>();

        services.AddTransient<ITokenProvider, JwtTokenProvider>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.Admin));

        services.AddScoped<PermissionManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<AdminAccountManager>();
        services.AddScoped<ParticipantAccountManager>();
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();
        services.AddScoped<IParticipantAccountManager, ParticipantAccountManager>();

        return services;
    }

    public static async Task ApplyAccountsMigration(this IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AccountsDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
