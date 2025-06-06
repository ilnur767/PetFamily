using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Specieses.Application;
using PetFamily.Specieses.Infrastructure.DbContexts;

namespace PetFamily.Specieses.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesesInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        services.AddScoped<ISpeciesReadDbContext, SpeciesReadDbContext>();
        services.AddScoped<SpeciesWriteDbContext>();

        return services;
    }

    public static async Task ApplySpecicesMigration(this IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
