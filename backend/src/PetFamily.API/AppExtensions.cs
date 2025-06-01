using Microsoft.EntityFrameworkCore;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.API;

public static class AppExtensions
{
    public static async Task ApplyMigration(this IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
