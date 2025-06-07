using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Volunteers.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersContract(this IServiceCollection services)
    {
        return services.AddScoped<IVolunteersContract, VolunteersContract>();
    }
}
