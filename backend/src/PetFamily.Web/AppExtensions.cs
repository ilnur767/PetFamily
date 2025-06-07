using PetFamily.Specieses.Infrastructure;
using PetFamily.Volunteers.Infrastructure;

namespace PetFamily.Web;

public static class AppExtensions
{
    public static async Task ApplyMigration(this IApplicationBuilder app)
    {
        await app.ApplySpecicesMigration();
        await app.ApplyVolunteersMigration();
    }
}
